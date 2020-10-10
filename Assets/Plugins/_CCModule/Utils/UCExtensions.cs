using UnityEngine;

public static class UnityExtensions
{
    #region UNITY3D内部方法扩展

    #region Vector3转Vector2

    /// <summary>
    /// 去除Y轴向
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns></returns>
    public static Vector2 IgnoreAxisY(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    /// <summary>
    /// 去除Z轴向
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns></returns>
    public static Vector2 IgnoreAxisZ(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }

    #endregion

    #region Transform类设置坐标

    public static Vector3 SetX(this Transform transform, float x)
    {
        return transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public static Vector3 SetY(this Transform tran, float y)
    {
        return tran.position = new Vector3(tran.position.x, y, tran.position.z);
    }

    public static Vector3 SetZ(this Transform transform, float z)
    {
        return transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    public static Vector3 SetLocalX(this Transform transform, float x)
    {
        return transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    public static Vector3 SetLocalY(this Transform transform, float y)
    {
        return transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }

    public static Vector3 SetLocalZ(this Transform transform, float z)
    {
        return transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }

    /// <summary>
    /// 设置局部坐标
    /// </summary>
    /// <param name="x">局部坐标x</param>
    /// <param name="y">局部坐标y</param>
    /// <param name="z">局部坐标z</param>
    public static Vector3 SetPosition(this Transform transform, float x, float y, float z)
    {
        return transform.localPosition = new Vector3(x, y, z);
    }

    /// <summary>
    /// 设置局部坐标
    /// </summary>
    /// <param name="vec3">局部坐标点</param>
    public static Vector3 SetPosition(this Transform transform, Vector3 vec3)
    {
        return transform.localPosition = vec3;
    }

    /// <summary>
    /// 设置局部缩放
    /// </summary>
    /// <param name="x">局部缩放值x</param>
    /// <param name="y">局部缩放值y</param>
    /// <param name="z">局部缩放值z</param>
    public static Vector3 SetScale(this Transform transform, float x, float y, float z)
    {
        return transform.localScale = new Vector3(x, y, z);
    }

    /// <summary>
    /// 设置局部缩放
    /// </summary>
    /// <param name="xAndYAndZ">局部缩放值x、y、z相等</param>
    public static Vector3 SetScale(this Transform transform, float xAndYAndZ)
    {
        return transform.localScale = new Vector3(xAndYAndZ, xAndYAndZ, xAndYAndZ);
    }

    /// <summary>
    /// 设置局部缩放
    /// </summary>
    /// <param name="vec3">局部缩放值</param>
    public static Vector3 SetScale(this Transform transform, Vector2 vec3)
    {
        return transform.localScale = vec3;
    }

    /// <summary>
    /// 设置局部坐标
    /// </summary>
    /// <param name="x">局部坐标x</param>
    /// <param name="y">局部坐标y</param>
    public static Vector2 SetPosition(this RectTransform transform, float x, float y)
    {
        return transform.anchoredPosition = new Vector2(x, y);
    }

    /// <summary>
    /// 设置局部坐标
    /// </summary>
    /// <param name="vec2">局部坐标点</param>
    public static Vector2 SetPosition(this RectTransform transform, Vector2 vec2)
    {
        return transform.anchoredPosition = vec2;
    }

    /// <summary>
    /// 设置局部缩放
    /// </summary>
    /// <param name="x">局部缩放值x</param>
    /// <param name="y">局部缩放值y</param>
    public static Vector2 SetScale(this RectTransform transform, float x, float y)
    {
        return transform.sizeDelta = new Vector2(x, y);
    }

    /// <summary>
    /// 设置局部缩放
    /// </summary>
    /// <param name="xAndY">局部缩放值x、y相等</param>
    public static Vector2 SetScale(this RectTransform transform, float xAndY)
    {
        return transform.sizeDelta = new Vector2(xAndY, xAndY);
    }

    /// <summary>
    /// 设置局部缩放
    /// </summary>
    /// <param name="vec2">局部缩放值</param>
    public static Vector2 SetScale(this RectTransform transform, Vector2 vec2)
    {
        return transform.sizeDelta = vec2;
    }

    /// <summary>
    /// 判断两个2D矩形是否相交
    /// 两个矩形相交的条件:两个矩形的中心距离在X和Y轴上都小于两个矩形长或宽的一半之和.
    /// </summary>
    public static bool Contains(this Transform transform, Transform otherTransform)
    {
        Vector3 smallOrthogonPos = transform.position;
        Vector3 smallOrthogonScale = transform.localScale;
        Vector3 moveOrthogonPos = otherTransform.position;
        Vector3 moveOrthogonScale = otherTransform.localScale;

        float halfSumX = (moveOrthogonScale.x * 0.5f) + (smallOrthogonScale.x * 0.5f);
        float halfSumY = (moveOrthogonScale.y * 0.5f) + (smallOrthogonScale.y * 0.5f);

        float distanceX = Mathf.Abs(moveOrthogonPos.x - smallOrthogonPos.x);
        float distanceY = Mathf.Abs(moveOrthogonPos.y - smallOrthogonPos.y);

        return distanceX <= halfSumX && distanceY <= halfSumY;
    }

    #endregion

    /// <summary>
    /// 获取或添加组件
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Component obj) where T : Component
    {
        T result = obj.GetComponent<T>();
        if (result == null) result = obj.gameObject.AddComponent<T>();
        return result;
    }

    /// <summary>
    /// 获取或添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        var result = obj.GetComponent<T>();
        if (result == null) result = obj.AddComponent<T>();
        return result;
    }

    #endregion
}