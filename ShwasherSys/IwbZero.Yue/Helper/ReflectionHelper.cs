using System;
using System.Linq;
using System.Reflection;

namespace IwbZero.Helper
{
    public static class ReflectionHelper
    {
        public static TOut GetFiledValue<TOut>(this object obj, string filedName)
        {
            try
            {
                var value = obj.GetType().GetProperty(filedName)?.GetValue(obj, null);
                return (TOut)value;
            }
            catch (Exception e)
            {
                //typeof(ReflectionHelper).LogError(e);
                return default(TOut);
            }
        }
        public static TOut SetFiledValue<TOut, TFiled>(this TOut obj, string filedName, TFiled value)
        {
            try
            {
                var filed = obj.GetType().GetProperty(filedName);
                if (filed != null)
                {
                    filed.SetValue(obj, value);
                }
            }
            catch (Exception e)
            {
                //typeof(ReflectionHelper).LogError(e);
            }
            return obj;
        }

        public static Type TypenFromAssembly(this Assembly assembly, string typeName)
        {
            Type[] typeArray = assembly.GetTypes();
            foreach (var type in typeArray)
            {
                if ((type.FullName != null && type.FullName.Equals(typeName)) || type.Name.Equals(typeName))
                {
                    return type;
                }
            }

            return null;
        }


        public static TAttribute GetSingleAttribute<TAttribute>(this Type type, TAttribute defaultValue = default(TAttribute), bool inherit = true)
            where TAttribute : Attribute
        {
            return type.GetCustomAttributes(inherit).OfType<TAttribute>().FirstOrDefault()
                   ?? type.ReflectedType?.GetTypeInfo().GetCustomAttributes(inherit).OfType<TAttribute>().FirstOrDefault()
                   ?? defaultValue;
        }
        public static TAttribute GetMemberSingleAttribute<TAttribute>(this MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true)
            where TAttribute : Attribute
        {
            //Get attribute on the member
            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First();
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets a property by it's full path from given object
        /// </summary>
        /// <param name="obj">Object to get value from</param>
        /// <param name="objectType">Type of given object</param>
        /// <param name="propertyPath">Full path of property</param>
        /// <returns></returns>
        internal static object GetPropertyByPath(object obj, Type objectType, string propertyPath)
        {
            var property = obj;
            var currentType = objectType;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (absolutePropertyPath.StartsWith(objectPath ?? throw new InvalidOperationException()))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            foreach (var propertyName in absolutePropertyPath.Split('.'))
            {
                if (currentType != null) property = currentType.GetProperty(propertyName);
                currentType = ((PropertyInfo)property)?.PropertyType;
            }

            return property;
        }

        /// <summary>
        /// Gets value of a property by it's full path from given object
        /// </summary>
        /// <param name="obj">Object to get value from</param>
        /// <param name="objectType">Type of given object</param>
        /// <param name="propertyPath">Full path of property</param>
        /// <returns></returns>
        internal static object GetValueByPath(object obj, Type objectType, string propertyPath)
        {
            var value = obj;
            var currentType = objectType;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (absolutePropertyPath.StartsWith(objectPath ?? throw new InvalidOperationException()))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            foreach (var propertyName in absolutePropertyPath.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                if (property != null)
                {
                    value = property.GetValue(value, null);
                    currentType = property.PropertyType;
                }
            }

            return value;
        }

        /// <summary>
        /// Sets value of a property by it's full path on given object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objectType"></param>
        /// <param name="propertyPath"></param>
        /// <param name="value"></param>
        internal static void SetValueByPath(object obj, Type objectType, string propertyPath, object value)
        {
            var currentType = objectType;
            PropertyInfo property;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (absolutePropertyPath.StartsWith(objectPath ?? throw new InvalidOperationException()))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            var properties = absolutePropertyPath.Split('.');

            if (properties.Length == 1)
            {
                property = objectType.GetProperty(properties.First());
                if (property != null) property.SetValue(obj, value);
                return;
            }

            for (int i = 0; i < properties.Length - 1; i++)
            {
                property = currentType.GetProperty(properties[i]);
                if (property != null)
                {
                    obj = property.GetValue(obj, null);
                    currentType = property.PropertyType;
                }
            }

            property = currentType.GetProperty(properties.Last());
            if (property != null) property.SetValue(obj, value);
        }
    }
}
