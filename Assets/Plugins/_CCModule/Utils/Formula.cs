using UnityEngine;

namespace CC
{
    public static class Formula
    {
        #region 判断数值有效性

        /// <summary>
        /// 是否是偶数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool IsEven(int a)
        {
            if (a % 2 == 0) return true;
            else return false;
        }

        #endregion

        #region 角度计算

        /// <summary>
        /// 角度
        /// 返回角度值并导出向量dir
        /// </summary>
        /// <param name="origion"></param>
        /// <param name="target"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static float GetAngle(Vector3 origion, Vector3 target, ref Vector3 dir)
        {
            Vector3 copyStandard = dir;
            dir = target - origion;
            if (dir.y > 0) return Vector3.Angle(copyStandard, dir);
            else return -Vector3.Angle(copyStandard, dir);
        }

        /// <summary>
        /// 原点到鼠标位置在摄像机上形成的角度
        /// 返回角度值并导出向量dir
        /// </summary>
        /// <param name="origion"></param>
        /// <param name="target"></param>
        /// <param name="dir"></param>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static float GetAngleFromPointToMouse(Vector3 origion, Vector3 target, ref Vector2 dir, Camera cam)
        {
            dir = cam.ScreenToWorldPoint(target) - origion;
            dir.x = dir.x < 0 ? -dir.x : dir.x;
            if (dir.y > 0) return Vector3.Angle(Vector3.right, dir);
            else return -Vector3.Angle(Vector3.right, dir);
        }

        #endregion

        #region 求点到直线的距离

        /// <summary>
        /// Vector3距离计算
        /// </summary>
        /// <param name="point"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static float DistanceToVector(Vector3 point, Vector3 startPoint, Vector3 endPoint)
        {
            Vector2 startVe2 = startPoint.IgnoreAxisY();
            Vector2 endVe2 = endPoint.IgnoreAxisY();
            float A = endVe2.y - startVe2.y;
            float B = startVe2.x - endVe2.x;
            float C = endVe2.x * startVe2.y - startVe2.x * endVe2.y;
            float denominator = Mathf.Sqrt(A * A + B * B);
            Vector2 pointVe2 = point.IgnoreAxisY();
            return Mathf.Abs((A * pointVe2.x + B * pointVe2.y + C) / denominator);
        }

        /// <summary>
        /// Vector2距离计算
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="vectorStart"></param>
        /// <param name="vectorEnd"></param>
        /// <returns></returns>
        public static float DistanceToVector(Vector2 origin, Vector2 vectorStart, Vector2 vectorEnd)
        {
            float A = vectorEnd.y - vectorStart.y;
            float B = vectorStart.x - vectorEnd.x;
            float C = vectorEnd.x * vectorStart.y - vectorStart.x * vectorEnd.y;
            float denominator = Mathf.Sqrt(A * A + B * B);

            return Mathf.Abs((A * origin.x + B * origin.y + C) / denominator);
        }

        #endregion

        /// <summary>
        /// 已知求抛物线坐标点
        /// </summary>
        /// <param name="length"></param>
        /// <param name="angle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static float GetParabolaPoint(float length, float angle = -5.0f, float offset = 2.5f)
        {
            return (Mathf.Pow(length, 2) / angle) + offset;
        }

        /// <summary>
        /// 求点是否在向量的左侧
        /// </summary>
        /// <param name="vector3"></param>
        /// <param name="originPoint"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool PointOnLeftSide(Vector3 vector3, Vector3 originPoint, Vector3 point)
        {
            Vector2 originVec2 = originPoint.IgnoreAxisY();
            Vector2 pointVec2 = (point.IgnoreAxisY() - originVec2).normalized;
            Vector2 vector2 = vector3.IgnoreAxisY();
            float verticalX = originVec2.x;
            float verticalY = (-verticalX * vector2.x) / vector2.y;
            Vector2 norVertical = (new Vector2(verticalX, verticalY)).normalized;
            float dotValue = Vector2.Dot(norVertical, pointVec2);
            return dotValue < 0f;
        }

        /// <summary>
        /// 求点是否在向量的左侧
        /// </summary>
        /// <param name="vector2"></param>
        /// <param name="originPoint"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool PointOnLeftSide(Vector2 vector2, Vector2 originPoint, Vector2 point)
        {
            Vector2 pointVec2 = (point - originPoint).normalized;
            float verticalX = originPoint.x;
            float verticalY = (-verticalX * vector2.x) / vector2.y;
            Vector2 norVertical = (new Vector2(verticalX, verticalY)).normalized;
            float dotValue = Vector2.Dot(norVertical, pointVec2);
            return dotValue < 0f;
        }
    }
}