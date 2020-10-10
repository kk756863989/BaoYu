using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Text;

public static class CSExtensions
{
    /// <summary>
    /// 扩展方法，获得枚举的Description
    /// enum Season 
    /// {
    ///     [Description("Spring")]
    ///     Spring = 1,
    /// }
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
    /// <returns>枚举的Description</returns>
    public static string GetDescription(this Enum value, Boolean nameInstead = true)
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);

        if (name == null)
        {
            return null;
        }

        FieldInfo field = type.GetField(name);
        DescriptionAttribute attribute =
            Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        if (attribute == null && nameInstead)
        {
            return name;
        }

        return attribute == null ? null : attribute.Description;
    }

    /// <summary>
    /// 把枚举转换为键值对集合
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    /// <param name="getText">以Enum为参数类型，String为返回类型的委托</param>
    /// <returns>以枚举值为key，枚举文本为value的键值对集合</returns>
    public static Dictionary<Int32, String> EnumToDictionary(Type enumType, Func<Enum, String> getText)
    {
        if (!enumType.IsEnum) throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");

        Dictionary<Int32, String> enumDic = new Dictionary<Int32, string>();
        Array enumValues = Enum.GetValues(enumType);

        foreach (Enum enumValue in enumValues)
        {
            Int32 key = Convert.ToInt32(enumValue);
            String value = getText(enumValue);
            enumDic.Add(key, value);
        }

        return enumDic;
    }

    /// <summary>
    /// Enum枚举随机
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="t">枚举类型</param>
    /// <returns>返回随机枚举类型</returns>
    public static T RandomEnum<T>(this T t) where T : struct
    {
        var type = typeof(T);

        if (type.IsEnum == false) throw new InvalidOperationException();

        var array = Enum.GetValues(type);
        var index = UnityEngine.Random.Range(array.GetLowerBound(0), array.GetUpperBound(0) + 1);
        return (T)array.GetValue(index);
    }

    /// <summary>
    /// 枚举索引转枚举类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T IntToEnum<T>(this int index)
    {
        return (T)Enum.Parse(typeof(T), index.ToString());
    }

    public static T IntToEnum<T>(this string index)
    {
        return (T)Enum.Parse(typeof(T), index);
    }

    /// <summary>
    /// 将枚举转换成索引号
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static int EnumToInt<T>(this T t)
    {
        return (int)(object)t;
    }

    /// <summary>
    /// 将枚举转换为字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string EnumToString<T>(this T t)
    {
        // solution1
        // return t.ToString();

        // solution2
        return Enum.GetName(t.GetType(), t);
    }

    /// <summary>
    /// 将字符串转换为枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <returns></returns>
    public static T StringToEnum<T>(this string str)
    {
        T t = default(T);

        try
        {
            t = (T)Enum.Parse(typeof(T), str);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return t;
    }

    /// <summary>
    /// 判断class是否实现某接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="myobj"></param>
    /// <param name="interfacename"></param>
    /// <returns></returns>
    public static bool IsRealizedInterface<T>(this T myobj, string interfacename)
    {
        if (myobj.GetType().GetInterface(interfacename) != null) return true;
        else return false;
    }

    /// <summary>
    /// 将整数格式化为1
    /// </summary>
    /// <param name="mInt"></param>
    /// <returns></returns>
    public static int Normalize(this int mInt)
    {
        if (mInt > 0) return 1;
        if (mInt < 0) return -1;
        return 0;
    }

    /// <summary>
    /// 将浮点数格式化为1
    /// </summary>
    /// <param name="mFloat"></param>
    /// <returns></returns>
    public static int Normalize(this float mFloat)
    {
        if (mFloat > 0) return 1;
        if (mFloat < 0) return -1;
        return 0;
    }

    /// <summary>
    /// 将HashTable转换为Json文本格式
    /// </summary>
    /// <param name="hashTable"></param>
    /// <returns></returns>
    public static string ToJsonData(this Hashtable hashTable)
    {
        try
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("{");

            foreach (String key in hashTable.Keys)
            {
                object value = hashTable[key];

                stringBuilder.Append("\"");
                stringBuilder.Append(key);
                stringBuilder.Append("\":\"");

                if (!String.IsNullOrEmpty(value.ToString()) && value != DBNull.Value)
                {
                    stringBuilder.Append(value).Replace("\\", "/");
                }
                else
                {
                    stringBuilder.Append(" ");
                }

                stringBuilder.Append("\",");
            }

            stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return "";
        }
    }
}