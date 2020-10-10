using System.Collections.Generic;

namespace CC
{
    public static class CachSystem
    {
        private static readonly Dictionary<string, CCPool> NamedCachPool;
        private static readonly Dictionary<int, CCPool> UnNamedCachPool;
        private static readonly List<string> PersistentCachPool;
        private static int poolCount;

        static CachSystem()
        {
            NamedCachPool = new Dictionary<string, CCPool>();
            UnNamedCachPool = new Dictionary<int, CCPool>();
            PersistentCachPool = new List<string>();
        }

        /// <summary>
        /// 添加为常驻对象池
        /// 只有命名的对象池可以设置为常驻
        /// </summary>
        /// <param name="poolName"></param>
        public static void MarkAsPersistent(string poolName)
        {
            if (PersistentCachPool.Contains(poolName)) return;
            PersistentCachPool.Add(poolName);
        }

        /// <summary>
        /// 添加匿名对象池
        /// </summary>
        /// <param name="ccPool"></param>
        /// <returns></returns>
        public static int Add(CCPool ccPool)
        {
            int uid = GenerateUid();

            UnNamedCachPool.Add(uid, ccPool);
            return uid;
        }

        public static int Add(IReuseObject poolModel, int modelCount = 1)
        {
            CCPool ccPool = new CCPool(poolModel, modelCount);
            int uid = GenerateUid();

            UnNamedCachPool.Add(uid, ccPool);
            return uid;
        }

        public static CCPool Add(string poolName, CCPool ccPool)
        {
            NamedCachPool.Add(poolName, ccPool);
            return ccPool;
        }

        public static CCPool Add(string poolName, IReuseObject poolModel, int modelCount = 1)
        {
            CCPool ccPool = new CCPool(poolModel, modelCount);
            NamedCachPool.Add(poolName, ccPool);
            return ccPool;
        }

        public static void Remove(string poolName)
        {
            if (!NamedCachPool.ContainsKey(poolName)) return;

            CCPool ccPool = NamedCachPool[poolName];

            ccPool.Release();
            NamedCachPool.Remove(poolName);
        }

        public static void Remove(int poolId)
        {
            if (!UnNamedCachPool.ContainsKey(poolId)) return;

            var ccPool = UnNamedCachPool[poolId];

            ccPool.Release();
            UnNamedCachPool.Remove(poolId);
        }

        public static void Clear()
        {
            ClearUnNamedPool();
            ClearNamedPool();
        }

        public static void ClearUnNamedPool()
        {
            foreach (var key in UnNamedCachPool.Keys)
            {
                Remove(key);
            }
        }

        public static void ClearNamedPool()
        {
            foreach (var key in NamedCachPool.Keys)
            {
                if (PersistentCachPool.Contains(key)) continue;

                Remove(key);
            }
        }

        /// <summary>
        /// 生成UID
        /// </summary>
        /// <returns></returns>
        private static int GenerateUid()
        {
            poolCount++;
            return poolCount;
        }
    }
}