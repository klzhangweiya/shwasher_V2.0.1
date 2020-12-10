using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShwasherSys
{
    public static class CommonHelper
    {
        public static string Obj2String(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                typeof(CommonHelper).LogError(e);
                return "";
            }
        }
        public static T GetResultModel<T>(this string objStr, bool isLower = true)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(objStr, converter);
                if (isLower)
                {
                    if (jsonObject.success)
                    {
                        T info = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonObject.result));
                        return info;
                    }
                }
                else
                {
                    if (jsonObject.Success)
                    {
                        T info = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonObject.Result));
                        return info;
                    }
                }

            }
            catch (Exception e)
            {
                typeof(CommonHelper).LogError(e);
            }
            return default(T);
        }

        public static T GetModel<T>(this string source)
        {

            try
            {
                if (string.IsNullOrEmpty(source))
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(source);
            }
            catch (Exception e)
            {
                typeof(CommonHelper).LogError(e);
                return default(T);
            }
        }

        public static T GetObj<T>(this string source, string key = null)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(source, converter);
                if (string.IsNullOrEmpty(key))
                {
                    T info = JsonConvert.DeserializeObject<T>(source);
                    return info;
                }
                object obj = jsonObject.GetValue(key);
                return (T)obj;
            }
            catch (Exception e)
            {
                typeof(CommonHelper).LogError(e);
                return default(T);
            }

        }
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="objs">The object.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        public static dynamic GetValue(this object objs, string key)
        {
            if (objs is List<object> oos)
            {
                foreach (var oo in oos)
                {
                    var result = GetValue(oo, key);//递归返回匹配的值
                    if (result != null)
                        return result;
                }
            }
            else if (objs is object[] objects)
            {
                foreach (var oo in objects)
                {
                    var result = GetValue(oo, key);//递归返回匹配的值
                    if (result != null)
                        return result;
                }
            }
            else if (objs is IDictionary<string, object> dictionarys)
            {
                foreach (var dic in dictionarys)
                {
                    if (dic.Key == key || dic.Key == key.ToLower() || dic.Key == key.ToUpper())
                        return dic.Value;
                }
                //如果上面的遍历没有结果，则该值可能嵌套在property.Value里面，需要递归解析
                foreach (var dic in dictionarys)
                {
                    var result = GetValue(dic.Value, key);//递归返回匹配的值
                    if (result != null)
                        return result;
                }
            }
            return null;//没有匹配值，返回null
        }
        public static int ToInt(this System.Enum e)
        {
            return e.GetHashCode();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
  
}
