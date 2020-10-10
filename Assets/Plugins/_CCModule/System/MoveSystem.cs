using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public static class MoveSystem
    {
        private class MoveHelper : MonoBehaviour
        {
            private void Update()
            {
                foreach (var m in moveTable)
                {
                    m.BeforeMove(Time.deltaTime);
                    m.OnMoving(Time.deltaTime);
                    m.AfterMove(Time.deltaTime);
                }
            }
        }

        private static List<IMove> moveTable = new List<IMove>();

        static MoveSystem()
        {
            Debug.Log("MoveSystem Awake");
            var helper = new GameObject("MoveHelper");
            helper.AddComponent<MoveHelper>();
            Object.DontDestroyOnLoad(helper);
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="iMove"></param>
        public static void Add(IMove iMove)
        {
            if (moveTable.Contains(iMove)) return;

            iMove.OnStartMove();
            moveTable.Add(iMove);
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="iMove"></param>
        public static void Remove(IMove iMove)
        {
            if (!moveTable.Contains(iMove)) return;

            iMove.OnStopMove();
            moveTable.Remove(iMove);
        }

        /// <summary>
        /// 清空元素
        /// 如需在清空前停止所有对象的移动，参数传入true即可
        /// </summary>
        /// <param name="stopDynamic"></param>
        public static void Clear(bool stopDynamic = false)
        {
            if (stopDynamic)
            {
                foreach (IMove m in moveTable)
                {
                    m.OnStopMove(Time.deltaTime);
                }
            }

            moveTable = new List<IMove>();
        }
    }
}