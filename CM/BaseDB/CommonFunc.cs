using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CMDB
{
    /// <summary>
    /// 公用方法
    /// </summary>
    public class CommonFunc
    {
        #region SQL

        //根据主键返回条件
        public static string GetSql<T>(object ParmaryValue)
        {
            string PrimaryName = "";
            ParmaryValue = UnInjection(ParmaryValue);//过滤关键字
            foreach (var pi in typeof(T).GetProperties())
            {
                if (pi != null)
                {
                    object[] pk = pi.GetCustomAttributes(false);
                    if (pk.Count() > 0)
                    {
                        Col_Attribute ca = pk[0] as Col_Attribute;
                        if (ca.IsPrimaryKey)//主键
                        {
                            PrimaryName = pi.Name;
                            break;
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(PrimaryName))
                throw new Exception("需要主键(请检查表是否设置主键以及MODEL主键字段是否设置特性)");
            else if (ParmaryValue == null)
                throw new Exception("请检查主键值");
            else
            {
                if ((ParmaryValue.GetType() == typeof(int) || ParmaryValue.GetType() == typeof(decimal) || ParmaryValue.GetType() == typeof(double) || ParmaryValue.GetType() == typeof(long) || ParmaryValue.GetType() == typeof(float)))
                    sb.Append(PrimaryName + "=" + ParmaryValue);
                else
                    sb.Append(PrimaryName + "='" + ParmaryValue + "'");
            }

            return sb.ToString(); ;
        }


        /// <summary>
        /// 根据数组 调用当前类的生成单条sql的方法生成多条sql（反射 调用不同类里的同一个方法来获得生成的sql）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="_class"></param>
        /// <returns></returns>
        public static string GetSql<T>(List<T> t, Object _class)
        {
            MethodInfo pi = null;
            MethodInfo[] pa = _class.GetType().GetMethods();//获得所有方法
            foreach (MethodInfo method in pa)
            {
                if (method.ToString() == "System.String CreateSql[T](T)")
                {
                    pi = method;
                    break;
                }
            }
            pi = pi.MakeGenericMethod(new Type[] { typeof(T) });
            StringBuilder sb = new StringBuilder();
            sb.Append("begin ");
            foreach (T item in t)
            {
                object[] parament = new object[1];
                parament[0] = item;
                sb.Append(pi.Invoke(_class, parament));
                sb.Append(";");
            }
            sb.Append("end;");
            return sb.ToString();
        }

        /// <summary>
        /// 关键字过滤
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object UnInjection(object obj)
        {
            if (obj == null) return null;
            if (obj.GetType() != typeof(string)) return obj;
            string Str = obj.ToString();
            return Str.Replace("select", "")
                .Replace("insert", "")
                 .Replace("update", "")
                  .Replace("delete", "")
                 .Replace("--", "")
                 .Replace("from", "")
                 .Replace("where", "")
                 .Replace("or", "")
                 .Replace("'", "");
        }
        #endregion

        #region Func<T,bool>
        /// <summary>
        /// 递归解析Lambda 生成对应SQL
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public static string Resolve(Expression express)
        {
            if (express is BinaryExpression)//但条件普通等式或多条件
            {
                BinaryExpression binary = express as BinaryExpression;

                /*************多条件***************/
                if (binary.Left is BinaryExpression)
                {
                    string _Field = Resolve(binary.Left);
                    string _Operators = ExpressTypeToStr(binary.NodeType);
                    object _Value = Resolve(binary.Right);

                    //多条件时判断 是否加括号
                    _Field = AddBrackets(_Field).ToString();
                    _Value = AddBrackets(_Value);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" " + _Field + " " + _Operators + " " + _Value);
                    return sb.ToString();
                }

                if (binary.Left is MemberExpression) //普通的等式或不等式条件
                {
                    MemberExpression me = (binary.Left as MemberExpression);
                    string _Field = me.Type == typeof(bool) ? me.Member.Name + " = 'true'" : me.Member.Name; //多条件 t.IsOk的情况
                    string _Operators = ExpressTypeToStr(binary.NodeType);
                    object _Value = null;
                    if (binary.Right is ConstantExpression || binary.Right is UnaryExpression || (binary.Right as MemberExpression).Expression is ConstantExpression)
                        _Value = getValue(binary.Right);
                    else
                        _Value = Resolve(binary.Right);

                    //多条件时判断 是否加括号
                    _Field = AddBrackets(_Field).ToString();
                    _Value = AddBrackets(_Value);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" " + _Field + " " + _Operators + " " + _Value);
                    return sb.ToString();
                }

                #region 特殊情况(多条件时递归 获得具体条件)
                /*********************Contains之类方法*********************=>!t.IsOk*********/
                if (binary.Left is MethodCallExpression || binary.Left is UnaryExpression)
                {
                    string _Field = Resolve(binary.Left);
                    string _Operators = ExpressTypeToStr(binary.NodeType);
                    object _Value = Resolve(binary.Right);

                    //多条件时判断 是否加括号
                    _Field = AddBrackets(_Field).ToString();
                    _Value = AddBrackets(_Value);

                    return " " + _Field + " " + _Operators + " " + _Value;

                }
                #endregion

            }



            #region 特殊情况解析
            if (express is MethodCallExpression) //Contains之类方法
            {
                MethodCallExpression methodcall = express as MethodCallExpression;
                string MethodName = methodcall.Method.Name;
                if (MethodName == "Contains")
                {
                    string _Field = (methodcall.Object as MemberExpression).Member.Name;
                    Expression argument = methodcall.Arguments[0];
                    string _likeValue = getValue(argument).ToString();
                    _likeValue = _likeValue.Replace("'", ""); //移除默认加的单引号
                    return " " + _Field + " like " + "'%" + _likeValue + "%'";
                }
                if (MethodName == "StartsWith")
                {
                    string _Field = (methodcall.Object as MemberExpression).Member.Name;
                    Expression argument = methodcall.Arguments[0];
                    string _likeValue = getValue(argument).ToString();
                    _likeValue = _likeValue.Replace("'", "");
                    return " " + _Field + " like " + "'" + _likeValue + "%'";
                }

                if (MethodName == "EndsWith")
                {
                    string _Field = (methodcall.Object as MemberExpression).Member.Name;
                    Expression argument = methodcall.Arguments[0];
                    string _likeValue = getValue(argument).ToString();
                    _likeValue = _likeValue.Replace("'", "");
                    return " " + _Field + " like " + "'%" + _likeValue + "'";
                }


            }
            if (express is MemberExpression) //=>t.IsOk
            {
                string _Field = (express as MemberExpression).Member.Name;
                string _Operators = "=";
                object _Value = "'true'";
                return " " + _Field + " " + _Operators + " " + _Value;
            }
            if (express is UnaryExpression)//=>!t.IsOk
            {
                UnaryExpression unary = express as UnaryExpression;
                if (unary.Operand is MemberExpression)
                {
                    string _Field = (unary.Operand as MemberExpression).Member.Name;
                    string _Operators = ExpressTypeToStr(express.NodeType);
                    object _Value = "'true'";
                    return " " + _Field + " " + _Operators + " " + _Value;
                }
            }
            #endregion


            throw new Exception("无法解析该表达式：" + express);
        }

        /// <summary>
        /// 加括号
        /// </summary>
        /// <param name="_obj"></param>
        /// <returns></returns>
        public static object AddBrackets(object _obj)
        {
            string _str = _obj.ToString();
            if (_str.Contains("or") || _str.Contains("and")) { }
            // return "(" + _obj + ")";

            return _obj;
        }

        public static object getValue(Expression express)
        {
            object _value = null;
            if (express is ConstantExpression)
                _value = (express as ConstantExpression).Value;

            else if (express is UnaryExpression)
            {
                LambdaExpression lambda = Expression.Lambda((express as UnaryExpression).Operand);
                Delegate fn = lambda.Compile();
                _value = fn.DynamicInvoke(null);
            }
            //解析变量值
            else if ((express as MemberExpression).Expression is ConstantExpression)
            {
                MemberExpression memberexpress = express as MemberExpression;
                object obj = (memberexpress.Expression as ConstantExpression).Value;
                if (memberexpress.Member is FieldInfo)
                    _value = (memberexpress.Member as FieldInfo).GetValue(obj);
                else if (memberexpress.Member is PropertyInfo)
                    _value = (memberexpress.Member as PropertyInfo).GetValue(obj);
            }

            if (_value == null) throw new Exception("无法解析该值：" + express);

            Type valueType = _value.GetType();
            if (valueType == typeof(string) || valueType == typeof(DateTime)) _value = "'" + _value + "'";
            if (valueType == typeof(bool)) _value = "'" + _value.ToString().ToLower() + "'"; //....................bool ToString会自动大写首字母
            return _value;
        }

        public static string ExpressTypeToStr(ExpressionType et)
        {
            switch (et)
            {
                case ExpressionType.And:
                    return "and";
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                    return "or";
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.MemberAccess: //特殊情况，不是等于 原意是：一个节点，表示从字段或属性读取。
                    return "=";
                case ExpressionType.Not:
                    return "!=";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                default:
                    throw new Exception("不支持" + et + "运算符查询");
            }
        }
        #endregion
    }
}
