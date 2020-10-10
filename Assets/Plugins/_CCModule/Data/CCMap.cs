using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace CC
{
    [Serializable]
    public sealed class CCMap : Dictionary<string, object>
    {
        #region 静态方法

        public static CCMap Read(string jsonName)
        {
            TextAsset text = Resources.Load<TextAsset>("Data/" + jsonName);

            if (text == null) return null;
            return Parse(text.text);
        }

        /// <summary>
        /// 将Json数据解析成Dictionary
        /// json以数组方式配置
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static CCMap Parse(string json)
        {
            return JsonConvert.DeserializeObject<CCMap>(json);
        }

        /// <summary>
        /// 转换成Json文本数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Stringify(CCMap data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 是否是字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsString<T>(T data)
        {
            if (data.GetType() != typeof(string)) return false;

            return true;
        }

        /// <summary>
        /// 是否是整型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsInt<T>(T data)
        {
            if (data.GetType() != typeof(Int32)) return false;

            return true;
        }

        /// <summary>
        /// 是否是浮点型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsFloat<T>(T data)
        {
            if (data.GetType() != typeof(float)) return false;

            return true;
        }

        #endregion

        /// <summary>
        /// 设置或添加键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Set<T>(string key, T value)
        {
            if (ContainsKey(key)) this[key] = value;
            else Add(key, value);

            return value;
        }

        /// <summary>
        /// int类型属性叠加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(string key, int value)
        {
            int result;

            try
            {
                result = (int) Query<long>(key);
            }
            catch (Exception ex)
            {
                result = Query<int>(key);
            }

            if (!IsInt(result)) return result;

            result += value;
            Set<int>(key, result);
            return result;
        }

        /// <summary>
        /// float类型属性叠加
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public float Add(string key, float value)
        {
            var result = Query<float>(key);

            if (!IsFloat(result)) return 0f;

            result += value;
            Set<float>(key, result);
            return result;
        }

        /// <summary>
        /// 根据键查询值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Query<T>(string key)
        {
            if (ContainsKey(key) && this[key] != null) return (T) (this[key]);

            return default;
        }

        /// <summary>
        /// 在数据中删除键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            if (!ContainsKey(key)) return false;
            Remove(key);
            return true;
        }
    }
}