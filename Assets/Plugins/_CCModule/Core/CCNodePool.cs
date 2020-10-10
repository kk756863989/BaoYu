using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CC
{
    public class CCNodePool
    {
        private GameObject model;
        private List<GameObject> cachPool = new List<GameObject>();
        private List<GameObject> activedPool = new List<GameObject>();

        public CCNodePool(GameObject item, int childNum = 1)
        {
            model = item;

            Put(item);

            if (childNum <= 1) return;
            Scale(childNum - 1);
        }

        /// <summary>
        /// 对象池扩充，需填入扩充数量
        /// </summary>
        /// <param name="addNum"></param>
        /// <returns></returns>
        private void Scale(int addNum)
        {
            GameObject instance;

            for (int i = 0; i < addNum; i++)
            {
                instance = Object.Instantiate(model, model.transform.parent, true);
                Put(instance);
            }
        }

        /// <summary>
        /// 
        /// 从对象池中获取闲置对象
        /// 1.首先通过 size 接口判断对象池中是否有空闲的对象
        /// 如果对象池可扩充则新增一个item
        /// 不可扩充,则将首个item重新放入对象池
        /// 2.获取一个闲置对象，并设置其坐标初始属性
        /// 
        /// </summary>
        /// <param name="scaleType"></param>
        /// <param name="position"></param>
        /// <param name="autoRecycleTime"></param>
        /// <param name="recycleDirection"></param>
        /// <returns></returns>
        public GameObject Add(PoolScaleType scaleType, Vector2 position, float autoRecycleTime = 0,
            int recycleDirection = 0)
        {
            GameObject node = Get(scaleType, recycleDirection);

            cachPool.Remove(node);
            activedPool.Add(node);
            node.SetActive(true);
            node.transform.localPosition = position;

            if (autoRecycleTime > 0)
            {
                TimerSystem.Add(TimerType.GAME, 1, autoRecycleTime * 10, Put, node);
            }

            return node;
        }

        /// <summary>
        /// 对象池回收全部实例
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            for (int i = activedPool.Count - 1; i >= 0; i--)
            {
                Put(activedPool[i]);
            }
        }

        /// <summary>
        /// 清空所有缓存对象和实例对象
        /// </summary>
        public void Release()
        {
            cachPool = cachPool.Concat(activedPool).ToList();

            for (int i = activedPool.Count - 1; i >= 0; i--)
            {
                Object.Destroy(activedPool[i]);
            }

            cachPool.Clear();
            activedPool.Clear();
        }

        /// <summary>
        /// 从对象池中取节点对象，不对此节点做任何处理
        /// </summary>
        /// <param name="scaleType"></param>
        /// <param name="recycleDirection"></param>
        /// <returns></returns>
        private GameObject Get(PoolScaleType scaleType, int recycleDirection = 0)
        {
            GameObject result = null;

            if (cachPool.Count == 0)
            {
                switch (scaleType)
                {
                    default:
                    case PoolScaleType.STATIC:
                        break;
                    case PoolScaleType.SCALEUP:
                        Scale(1);
                        result = GetHeadCach();
                        break;
                    case PoolScaleType.RECYCLE:
                        Recycle(recycleDirection);
                        result = GetHeadCach();
                        break;
                }
            }
            else
            {
                result = GetHeadCach();
            }

            return result;
        }

        /// <summary>
        /// 对象池回收实例
        /// </summary>
        /// <param name="args"></param>
        public void Put(params object[] args)
        {
            GameObject instance = args[0] as GameObject;

            if (instance == null) return;

            if (activedPool.Contains(instance)) activedPool.Remove(instance);
            cachPool.Add(instance);
            instance.SetActive(false);
        }

        /// <summary>
        /// 回收首个节点
        /// </summary>
        /// <param name="recycleDirection"></param>
        /// <returns></returns>
        private void Recycle(int recycleDirection = 0)
        {
            GameObject instance = recycleDirection == 0 ? GetHeadInstance() : GetRearInstance();

            if (instance.Equals(null)) Debug.Log((object)"Actived pool is empty.Nothing can be recycled");

            Put(instance);
        }

        /// <summary>
        /// 获取首个实例
        /// </summary>
        /// <returns></returns>
        private GameObject GetHeadInstance()
        {
            if (GetInstanceCount() <= 0) return null;
            return activedPool[0];
        }

        /// <summary>
        /// 获取最后一个实例
        /// </summary>
        /// <returns></returns>
        private GameObject GetRearInstance()
        {
            if (GetInstanceCount() == 0) return null;
            return activedPool[activedPool.Count - 1];
        }

        /// <summary>
        /// 获取最后一个缓存对象
        /// </summary>
        /// <returns></returns>
        private GameObject GetRearCach()
        {
            if (GetCachCount() == 0) return null;
            return cachPool[cachPool.Count - 1];
        }

        /// <summary>
        /// 获取第一个缓存对象
        /// </summary>
        /// <returns></returns>
        private GameObject GetHeadCach()
        {
            if (GetCachCount() == 0) return null;
            return cachPool[0];
        }

        /// <summary>
        /// 获取当前对象池内的实例数量
        /// </summary>
        /// <returns></returns>
        private int GetInstanceCount()
        {
            return activedPool.Count;
        }

        private int GetCachCount()
        {
            return cachPool.Count;
        }
    }
}