using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys
{
    public static class ListHelper
    {

        /// <summary>
        /// 自定义Distinct扩展方法
        /// </summary>
        /// <typeparam name="T">要去重的对象类</typeparam>
        /// <typeparam name="C">自定义去重的字段类型</typeparam>
        /// <param name="source">要去重的对象</param>
        /// <param name="getField">获取自定义去重字段的委托</param>
        /// <returns></returns>
        public static IEnumerable<T> MyDistinct<T, C>(this IEnumerable<T> source, Func<T, C> getField)
        {
            return source.Distinct(new Compare<T, C>(getField));
        }
    }
    public class Compare<T, C> : IEqualityComparer<T>
    {
        private Func<T, C> GetField { get; set; }
        public Compare(Func<T, C> getField)
        {
            GetField = getField;
        }
        public bool Equals(T x, T y)
        {
            return EqualityComparer<C>.Default.Equals(GetField(x), GetField(y));
        }
        public int GetHashCode(T obj)
        {
            return EqualityComparer<C>.Default.GetHashCode(GetField(obj));
        }
    }
}
