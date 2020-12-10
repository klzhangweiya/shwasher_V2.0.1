using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ShwasherSys.Lambda
{
    public static class LambdaHelper
    {
        /// <summary>
        /// 创建 LambdaExpression<Func<T, bool>>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetExp<T>(this LambdaObject obj, ParameterExpression left = null)
        {
            Expression<Func<T, bool>> finalExp = null;
            left = left ?? Expression.Parameter(typeof(T), "a");
            Expression right = GetRightExp<T>(obj, left);
            if (right != null) finalExp = Expression.Lambda<Func<T, bool>>(right, left);
            return finalExp;
        }
        /// <summary>
        /// 创建 LambdaExpression<Func<T, bool>>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetExp<T>(this List<LambdaObject> objs, ParameterExpression left = null)
        {
            Expression<Func<T, bool>> finalExp = null;
            left = left ?? Expression.Parameter(typeof(T), "a");
            Expression right = null;
            foreach (var obj in objs)
            {
                Expression exp = GetRightExp<T>(obj, left);
                right = right == null ? exp : CombineExp(right, exp, obj.LogicType);
            }
            if (right != null) finalExp = Expression.Lambda<Func<T, bool>>(right, left);
            return finalExp;
        }

        /// <summary>
        /// 创建 MemberExpression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Expression GetRightExp<T>(LambdaObject obj, ParameterExpression left)
        {
            left = left ?? Expression.Parameter(typeof(T), "a");//创建参数a

            string fieldName = obj.FieldName;
            //var fieldValue = obj.FieldValue;
            MemberExpression member = Expression.PropertyOrField(left, fieldName);
            Expression rignt = null;
            if (obj.Children != null && obj.Children.Any())
                rignt = GetRightExps<T>(obj.Children, left);
            else
            {
                ConstantExpression constant = GetConstantExp<T>(obj);
                switch ((int)obj.ExpType)
                {
                    case 0:
                        rignt = constant != null ? Expression.Equal(member, constant) : null;
                        break;
                    case 1:
                        rignt = constant != null ? Expression.NotEqual(member, constant) : null;
                        break;
                    case 2:
                        rignt = constant != null ? Expression.GreaterThan(member, constant) : null;
                        break;
                    case 3:
                        rignt = constant != null ? Expression.LessThan(member, constant) : null;
                        break;
                    case 4:
                        rignt = constant != null ? Expression.GreaterThanOrEqual(member, constant) : null;
                        break;
                    case 5:
                        rignt = constant != null ? Expression.LessThanOrEqual(member, constant) : null;
                        break;
                    case 6:
                        rignt = GetRightExp_Contains<T>(obj, left);
                        break;
                    case 7:
                        rignt = GetRightExp_NotContains<T>(obj, left);
                        break;
                    case 8:
                        break;
                }
            }
            return rignt;
        }
        /// <summary>
        /// 创建 MemberExpression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Expression GetRightExps<T>(List<LambdaObject> objs, ParameterExpression left)
        {
            left = left ?? Expression.Parameter(typeof(T), "a");//创建参数a
            Expression right = null;
            foreach (var obj in objs)
            {
                Expression exp = GetRightExp<T>(obj, left);
                right = right == null ? exp : CombineExp(right, exp, obj.LogicType);
            }
            return right;
        }

        /// <summary>
        /// 创建 ConstantExpression
        /// </summary>
        private static ConstantExpression GetConstantExp<T>(LambdaObject obj)
        {
            var fieldValue = obj.FieldValue;
            ConstantExpression constant = null;
            switch ((int)obj.FieldType)
            {
                case 0:
                    constant = Expression.Constant(fieldValue is string ? fieldValue : fieldValue.ToString(), typeof(string));
                    break;
                case 1:
                    if (fieldValue is int)
                        constant = Expression.Constant(fieldValue, typeof(int));
                    else if (int.TryParse(fieldValue.ToString(), out int newValue))
                        constant = Expression.Constant(newValue, typeof(int));
                    break;
                case 2:
                    if (fieldValue is int)
                        constant = Expression.Constant(fieldValue, typeof(int?));
                    else if (int.TryParse(fieldValue.ToString(), out int newValue))
                        constant = Expression.Constant(newValue, typeof(int?));
                    break;
                case 3:
                    if (fieldValue is bool)
                        constant = Expression.Constant(fieldValue, typeof(bool));
                    else if (bool.TryParse(fieldValue.ToString(), out bool newValue))
                        constant = Expression.Constant(newValue, typeof(bool));
                    break;
                case 4:
                    if (fieldValue is bool)
                        constant = Expression.Constant(fieldValue, typeof(bool?));
                    else if (bool.TryParse(fieldValue.ToString(), out bool newValue))
                        constant = Expression.Constant(newValue, typeof(bool?));
                    break;
                case 5:
                    if (fieldValue is DateTime)
                        constant = Expression.Constant(fieldValue, typeof(DateTime));
                    else if (DateTime.TryParse(fieldValue.ToString(), out DateTime newValue))
                        constant = Expression.Constant(newValue, typeof(DateTime));
                    break;
                case 6:
                    if (fieldValue is DateTime)
                        constant = Expression.Constant(fieldValue, typeof(DateTime?));
                    else if (DateTime.TryParse(fieldValue.ToString(), out DateTime newValue))
                        constant = Expression.Constant(newValue, typeof(DateTime?));
                    break;
                case 7:
                    if (fieldValue is decimal)
                        constant = Expression.Constant(fieldValue, typeof(decimal));
                    else if (decimal.TryParse(fieldValue.ToString(), out decimal newValue))
                        constant = Expression.Constant(newValue, typeof(decimal));
                    break;
                case 8:
                    if (fieldValue is decimal?)
                        constant = Expression.Constant(fieldValue, typeof(decimal?));
                    else if (decimal.TryParse(fieldValue.ToString(), out decimal newValue))
                        constant = Expression.Constant(newValue, typeof(decimal?));
                    break;
            }
            return constant;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.Contains(propertyValue)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Expression GetRightExp_Contains<T>(LambdaObject obj, ParameterExpression left = null)
        {
            string fieldName = obj.FieldName;
            var fieldValue = obj.FieldValue;
            left = left ?? Expression.Parameter(typeof(T), "a");//创建参数a
            MemberExpression member = Expression.PropertyOrField(left, fieldName);
            Expression right = null;
            switch (obj.FieldType)
            {
                case LambdaFieldType.S:
                    var method1 = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    if (method1 == null) return null;
                    if (fieldValue is string)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue.ToString());//创建常数
                        right = Expression.Call(member, method1, constant);
                    }
                    else
                    {
                        foreach (var o in (IEnumerable)fieldValue)
                        {
                            ConstantExpression constant = Expression.Constant(o);
                            var temp = Expression.Call(member, method1, constant);
                            right = right == null ? temp : CombineExp(right, temp, LogicType.Or);
                        }
                    }
                    break;
                case LambdaFieldType.I:
                    var method2 = typeof(int).GetMethod("Contains", new[] { typeof(int) });
                    if (method2 == null) return null;
                    if (fieldValue is int)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue.ToString());
                        right = Expression.Call(member, method2, constant);
                    }
                    else
                    {
                        foreach (var o in (IEnumerable)fieldValue)
                        {
                            ConstantExpression constant = Expression.Constant(o);
                            var temp = Expression.Call(member, method2, constant);
                            right = right == null ? temp : CombineExp(right, temp, LogicType.Or);
                        }
                    }
                    break;
                case LambdaFieldType.B:
                    var method3 = typeof(bool).GetMethod("Contains", new[] { typeof(bool) });
                    if (method3 == null) return null;
                    if (fieldValue is bool)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue.ToString());
                        right = Expression.Call(member, method3, constant);
                    }
                    break;
            }
            return right;
        }

        /// <summary>
        /// 创建lambda表达式：!(p=>p.propertyName.Contains(propertyValue))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Expression GetRightExp_NotContains<T>(LambdaObject obj, ParameterExpression left = null)
        {
            return Expression.Not(GetRightExp_Contains<T>(obj, left));
        }


        /// <summary>
        /// 拼接 Expression
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Expression CombineExp(this Expression exp1, Expression exp2, LogicType type)
        {
            Expression exp = exp1;
            if (exp1 == null || exp2 == null)
            {
                return exp1;
            }
            switch ((int)type)
            {
                case 0:
                    exp = Expression.And(exp1, exp2);
                    break;
                case 1:
                    exp = Expression.Or(exp1, exp2);
                    break;
            }
            return exp;
        }

        #region  暂未用到

        /// <summary>
        /// 创建 MemberExpression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Expression GetRightExp2<T>(List<LambdaObject> objs, ParameterExpression left)
        {
            Expression right = null;
            foreach (var obj in objs)
            {
                Expression exp = null;
                if (obj.Children.Any())
                {
                    exp = GetRightExps<T>(obj.Children, left);
                    right = right == null ? exp : CombineExp(right, exp, obj.LogicType);
                    continue;
                }
                switch ((int)obj.ExpType)
                {
                    case 0:
                        exp = GetRightExp_Equal<T>(obj, left);
                        break;
                    case 1:
                        exp = GetRightExp_NotEqual<T>(obj, left);
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        exp = GetRightExp_Contains<T>(obj);
                        break;
                    case 8:
                        exp = GetRightExp_NotContains<T>(obj);
                        break;
                }
                right = right == null ? exp : CombineExp(right, exp, obj.LogicType);
            }
            return right;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        public static Expression GetRightExp_Equal<T>(LambdaObject obj, ParameterExpression left = null)
        {
            //Expression<Func<T, bool>> exp = null;
            string fieldName = obj.FieldName;
            var fieldValue = obj.FieldValue;
            left = left ?? Expression.Parameter(typeof(T), "a");//创建参数a
            MemberExpression member = Expression.PropertyOrField(left, fieldName);
            Expression right = null;
            switch ((int)obj.FieldType)
            {
                case 0:
                    if (fieldValue is string)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue);//创建常数
                                                                                      //exp= Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), left);
                        right = Expression.Equal(member, constant);
                    }
                    //else if (os is List<string>)
                    //{
                    //	foreach (var o in (List<string>) os)
                    //	{
                    //		ConstantExpression constant = Expression.Constant(o);
                    //		var temp = Expression.Equal(member, constant);
                    //		right = CombineExp(right,temp,obj.LogicType);
                    //	}
                    //}
                    break;
                case 1:
                    if (fieldValue is int)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue);
                        right = Expression.Equal(member, constant);
                    }
                    break;
                case 2:
                    if (fieldValue is bool)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue);
                        right = Expression.Equal(member, constant);
                    }
                    break;
            }
            return right;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        public static Expression GetRightExp_NotEqual<T>(LambdaObject obj, ParameterExpression left = null)
        {
            string fieldName = obj.FieldName;
            var fieldValue = obj.FieldValue;
            left = left ?? Expression.Parameter(typeof(T), "a");//创建参数a
            MemberExpression member = Expression.PropertyOrField(left, fieldName);
            Expression right = null;
            switch ((int)obj.FieldType)
            {
                case 0:
                    if (fieldValue is string)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue);
                        right = Expression.NotEqual(member, constant);
                    }
                    break;
                case 1:
                    if (fieldValue is int)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue);
                        right = Expression.NotEqual(member, constant);
                    }
                    break;
                case 2:
                    if (fieldValue is bool)
                    {
                        ConstantExpression constant = Expression.Constant(fieldValue);
                        right = Expression.NotEqual(member, constant);
                    }
                    break;
            }
            return right;
        }

        /// <summary>
        /// 创建lambda表达式：p=>true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return p => true;
        }

        /// <summary>
        /// 创建lambda表达式：p=>false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return p => false;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateNotEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName > propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThan<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName < propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThan<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName >= propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName <= propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThanOrEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.Contains(propertyValue)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：!(p=>p.propertyName.Contains(propertyValue))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
        }
        #endregion
    }

    public class LambdaObject
    {
        public LambdaObject()
        {

        }
        public LambdaObject(string fieldName, object fieldValue, LambdaExpType expType = LambdaExpType.Contains, LambdaFieldType fieldType = LambdaFieldType.S, LogicType logicType = LogicType.And)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
            FieldType = fieldType;
            ExpType = expType;
            LogicType = logicType;
        }
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public LambdaFieldType FieldType { get; set; }
        public LambdaExpType ExpType { get; set; }
        public LogicType LogicType { get; set; }
        public List<LambdaObject> Children { get; set; }
    }
    /// <summary>
    /// 表达式类型
    /// </summary>
    public enum LambdaExpType
    {
        Equal
        , NotEqual
        , Greater
        , Less
        , GreaterOrEqual
        , LessOrEqual
        , Contains
        , NotContains
    }
    /// <summary>
    /// 字段类型
    /// </summary>
    public enum LambdaFieldType
    {
        /// <summary>
        /// string
        /// </summary>
        S = 0,
        /// <summary>
        /// int
        /// </summary>
        I = 1,
        /// <summary>
        /// int?
        /// </summary>
        Inull = 2,
        /// <summary>
        /// bool
        /// </summary>
        B = 3,
        /// <summary>
        /// bool?
        /// </summary>
        Bnull = 4,
        /// <summary>
        /// Datetime
        /// </summary>
        D = 5,
        /// <summary>
        /// Datetime?
        /// </summary>
        Dnull = 6,
        /// <summary>
        /// Decimal
        /// </summary>
        Dec = 7,
        /// <summary>
        /// Decimal?
        /// </summary>
        DecNull = 8
    }
    /// <summary>
    /// 拼接时 AND || OR
    /// </summary>
    public enum LogicType
    {
        And,
        Or
    }
}
