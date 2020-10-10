using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CC
{
    public enum PoolScaleType
    {
        STATIC = 0, //保持不变
        SCALEUP = 1, //自动放大
        RECYCLE = 2 //自动回收
    }

    public class CCPool
    {
        private IReuseObject model;
        private List<IReuseObject> cachPool = new List<IReuseObject>();
        private List<IReuseObject> activedPool = new List<IReuseObject>();

        public CCPool(IReuseObject item, int childNum = 1)
        {
            model = item;

            item.Init();
            Put(item);

            if (childNum <= 1) return;
            Scale(childNum - 1);
        }

        /// <summary>
        /// 从对象池中获取闲置对象
        /// 1.首先通过 size 接口判断对象池中是否有空闲的对象
        /// 如果对象池可扩充则新增一个item
        /// 不可扩充,则将首个item重新放入对象池
        /// 2.获取一个闲置对象，并设置其坐标初始属性
        /// </summary>
        /// <param name="scaleType"></param>
        /// <param name="position"></param>
        /// <param name="recycleDirection"></param>
        /// <returns></returns>
        public IReuseObject Add(PoolScaleType scaleType, Vector2 position, int recycleDirection = 0)
        {
            IReuseObject node = Get(scaleType, recycleDirection);

            cachPool.Remove(node);
            activedPool.Add(node);
            return node;
        }

        /// <summary>
        /// 对象池回收全部实例
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            cachPool = cachPool.Concat(activedPool).ToList();
            activedPool.Clear();
        }

        /// <summary>
        /// 清空所有缓存对象和实例对象
        /// </summary>
        public void Release()
        {
            cachPool.Clear();
            activedPool.Clear();
        }

        /// <summary>
        /// 对象池扩充，需填入扩充数量
        /// </summary>
        /// <param name="addNum"></param>
        /// <returns></returns>
        private void Scale(int addNum)
        {
            IReuseObject instance = null;

            for (var i = 0; i < addNum; i++)
            {
                var t = model.GetType();

                if (t.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    var mb = model as MonoBehaviour;
                    if (mb != null)
                    {
                        var obj = Object.Instantiate(mb.gameObject);
                        instance = obj.GetComponent<MonoBehaviour>() as IReuseObject;
                    }
                }
                else instance = Activator.CreateInstance(t) as IReuseObject;

                if (instance == null) continue;
                instance.Init();
                Put(instance);
            }
        }

        /// <summary>
        /// 从对象池中取节点对象，不对此节点做任何处理
        /// </summary>
        /// <param name="scaleType"></param>
        /// <param name="recycleDirection"></param>
        /// <returns></returns>
        private IReuseObject Get(PoolScaleType scaleType, int recycleDirection = 0)
        {
            IReuseObject result;

            if (cachPool.Count == 0)
            {
                switch (scaleType)
                {
                    default:
                        return null;
                    case PoolScaleType.SCALEUP:
                        Scale(1);
                        result = GetHeadCach();
                        break;
                    case PoolScaleType.RECYCLE:
                        Recycle(recycleDirection);
                        result = GetHeadCach();
                        result.Init();
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
        /// <param name="instance"></param>
        /// <returns></returns>
        public IReuseObject Put(IReuseObject instance)
        {
            if (activedPool.Contains(instance)) activedPool.Remove(instance);
            cachPool.Add(instance);
            instance.OnRecycle();
            return instance;
        }

        /// <summary>
        /// 回收首个节点
        /// </summary>
        /// <param name="recycleDirection"></param>
        /// <returns></returns>
        private void Recycle(int recycleDirection = 0)
        {
            IReuseObject instance = recycleDirection == 0 ? GetHeadInstance() : GetRearInstance();

            if (instance.Equals(null)) Debug.Log((object)"Actived pool is empty.Nothing can be recycled");

            Put(instance);
        }

        /// <summary>
        /// 获取首个实例
        /// </summary>
        /// <returns></returns>
        private IReuseObject GetHeadInstance()
        {
            return GetInstanceCount() <= 0 ? null : activedPool[0];
        }

        /// <summary>
        /// 获取最后一个实例
        /// </summary>
        /// <returns></returns>
        private IReuseObject GetRearInstance()
        {
            return GetInstanceCount() == 0 ? null : activedPool[activedPool.Count - 1];
        }

        /// <summary>
        /// 获取最后一个缓存对象
        /// </summary>
        /// <returns></returns>
        private IReuseObject GetRearCach()
        {
            return GetCachCount() == 0 ? null : cachPool[cachPool.Count - 1];
        }

        /// <summary>
        /// 获取第一个缓存对象
        /// </summary>
        /// <returns></returns>
        private IReuseObject GetHeadCach()
        {
            return GetCachCount() == 0 ? null : cachPool[0];
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