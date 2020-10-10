using UnityEngine;

namespace CC
{
    /// <summary>
    /// 排序模式
    /// </summary>
    public enum SortMode
    {
        INCREASE = 0, //递增
        DECREASE = 1 //递减
    }

    public static class Utils
    {
        // 是否处于移动方向上
        public static bool IsOnDirection(Vector3 curPos, Vector3 fromPos, Vector3 tagertPos)
        {
            return (tagertPos - fromPos).normalized.Equals((tagertPos - curPos).normalized);
        }

        #region 角色常用逻辑

        /// <summary>
        /// 设置距离地面的高度
        /// </summary>
        /// <param name="point"></param>
        /// <param name="offsetHeight"></param>
        /// <param name="upY"></param>
        /// <param name="downY"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static Vector2 SetYAwayRoad(Vector2 point, float offsetHeight = 0, float upY = 500, float downY = 800,
            string layerName = "Road")
        {
            var tempPoint = point;
            tempPoint.y = upY;
            var hit = Physics2D.CircleCast(tempPoint, 0.1f, Vector3.down, downY, LayerMask.GetMask(layerName));

            if (hit) point = hit.point + new Vector2(0, offsetHeight);
            return point;
        }

        /// <summary>
        /// 获取距离地面的高度
        /// 当悬空无路面时将返回正无穷
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="r"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static float GetYAwayRoad(Transform tran, float r, params string[] layerName)
        {
            float result;

            var hit = Physics2D.CircleCast(tran.position + Vector3.up * r, r, Vector2.down, 1000,
                LayerMask.GetMask(layerName));

            if (!hit) result = float.PositiveInfinity;
            else if (hit.point.y < tran.position.y) result = tran.position.y - hit.point.y;
            else
            {
                if (hit.point.y - tran.position.y < 0.3f) result = 0;
                else result = hit.point.y - tran.position.y;
            }

            return result;
        }

        #endregion

        #region 2DMesh序列化层

        /// <summary>
        /// 重新设定物件所有渲染组件的渲染层id
        /// 渲染层id最高不超过设定值maxNum
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="maxNum"></param>
        /// <param name="sortLayerName"></param>
        /// <param name="mode"></param>
        public static void SortLayer(GameObject obj, int maxNum = 0, string sortLayerName = "Default",
            SortMode mode = SortMode.DECREASE)
        {
            Renderer[] list = obj.GetComponentsInChildren<Renderer>();
            if (list.Length == 0) return;
            PopSortRenders(list, mode);
            for (int i = 0; i < list.Length; i++)
            {
                list[i].sortingLayerName = sortLayerName;
                list[i].sortingOrder = maxNum - i;
            }
        }

        /// <summary>
        /// 对渲染数组进行排序
        /// 默认按渲染层id由高到低排列
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="mode"></param>
        public static void PopSortRenders(Renderer[] arr, SortMode mode = SortMode.DECREASE)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                var frontArrId = i - 1;
                var currentArrId = i;
                while (frontArrId >= 0)
                {
                    Renderer tempRender;
                    switch (mode)
                    {
                        case SortMode.DECREASE:
                            if (arr[currentArrId].sortingOrder > arr[frontArrId].sortingOrder)
                            {
                                tempRender = arr[currentArrId];
                                arr[currentArrId] = arr[frontArrId];
                                arr[frontArrId] = tempRender;
                            }

                            break;
                        case SortMode.INCREASE:
                            if (arr[currentArrId].sortingOrder < arr[frontArrId].sortingOrder)
                            {
                                tempRender = arr[currentArrId];
                                arr[currentArrId] = arr[frontArrId];
                                arr[frontArrId] = tempRender;
                            }

                            break;
                    }

                    frontArrId--;
                }
            }
        }

        #endregion
    }
}