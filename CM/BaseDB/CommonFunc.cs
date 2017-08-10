using System;
using System.Collections;
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
        /// 递归解析Lambda 生成对应SQL !bool bool 未解决
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public static string Resolve(Expression express)
        {
            #region 解析
            //二元表达式，比如a.id == 1
            if (express is BinaryExpression)
            {
                BinaryExpression binary = express as BinaryExpression;

                string _Field = Resolve(binary.Left);
                string _Operators = ExpressTypeToStr(binary.NodeType);
                object _Value = Resolve(binary.Right);
                return _Field + " " + _Operators + " " + _Value;
            }

            //一元表达式 Convert(num)
            if (express is UnaryExpression)
            {
                UnaryExpression unary = express as UnaryExpression;
                string unarystr = Resolve(unary.Operand);
                string _Operators = ExpressTypeToStr(express.NodeType);

                return _Operators + " " + unarystr;
            }

            //常量表达式 True,1,2,3
            if (express is ConstantExpression)
            {
                ConstantExpression constant = express as ConstantExpression;

                if (constant.Type.Name == "String" || constant.Type.Name == "DateTime")
                    return "'" + constant.Value.ToString() + "'";
                else if (constant.Type.Name == "Nullable`1")
                    return "null";
                else if (constant.Type.Name == "Boolean")
                    return "'" + constant.Value.ToString().ToLower() + "'";
                else
                    return constant.Value.ToString();
            }

            //函数表达式 Contains之类方法
            if (express is MethodCallExpression)
            {
                MethodCallExpression methodcall = express as MethodCallExpression;
                string MethodName = methodcall.Method.Name;
                if (MethodName == "ToString") return Resolve(methodcall.Object);

                #region Contains的in
                if (MethodName == "Contains" && methodcall.Object.Type.Name != "String")
                {
                    string _tempField = Resolve(methodcall.Arguments[0]);

                    var _tempList = Expression.Lambda<Func<object>>(methodcall.Object).Compile()() as IList;
                    var list = (from object i in _tempList select "'" + i + "'").ToList();
                    return _tempField + " in (" + string.Join(",", list.Cast<string>().ToArray()) + ") ";
                }
                #endregion

                if (MethodName == "Parse")// && express.Type == typeof(Guid))
                {
                    switch (methodcall.Type.Name)
                    {
                        case "Int16":
                            return Expression.Lambda<Func<short>>(methodcall).Compile()().ToString();
                        case "Int32":
                            return Expression.Lambda<Func<int>>(methodcall).Compile()().ToString();
                        case "Int64":
                            return Expression.Lambda<Func<long>>(methodcall).Compile()().ToString();
                        case "Double":
                            return Expression.Lambda<Func<double>>(methodcall).Compile()().ToString();
                        case "Decimal":
                            return Expression.Lambda<Func<decimal>>(methodcall).Compile()().ToString();
                        case "String":
                            return "'" + Expression.Lambda<Func<string>>(methodcall).Compile()() + "'";
                        case "DateTime":
                            return "'" + Expression.Lambda<Func<DateTime>>(methodcall).Compile()() + "'";
                        case "Guid":
                                return "'" + BitConverter.ToString(Expression.Lambda<Func<Guid>>(methodcall).Compile()().ToByteArray()).ToString().Replace("-", "") + "'";
                        default:
                            return Expression.Lambda<Func<object>>(methodcall).Compile()().ToString();
                    }
                }

                string _Field = Resolve(methodcall.Object);
                Expression argument = methodcall.Arguments[0];
                string _likeValue = Resolve(argument);
                _likeValue = _likeValue.Replace("'", ""); //移除默认加的单引号

                if (MethodName == "Contains")
                    return _Field + " like " + "'%" + _likeValue + "%'";
                if (MethodName == "StartsWith")
                    return _Field + " like " + "'" + _likeValue + "%'";
                if (MethodName == "EndsWith")
                    return _Field + " like " + "'%" + _likeValue + "'";
            }

            //成员表达式，一般为变量，比如a.id
            if (express is MemberExpression)
            {
                MemberExpression member = express as MemberExpression;

                //DateTime.Now的情况
                if (member.Type.Name == "DateTime") return "'" + Expression.Lambda<Func<DateTime>>(member).Compile()() + "'";

                //普通字段
                if (member.Expression.GetType().Name == "TypedParameterExpression")
                    return member.Expression.Type.Name + "." + member.Member.Name;

                //值
                switch (member.Type.Name)
                {
                    case "Int16":
                        return Expression.Lambda<Func<short>>(member).Compile()().ToString();
                    case "Int32":
                        return Expression.Lambda<Func<int>>(member).Compile()().ToString();
                    case "Int64":
                        return Expression.Lambda<Func<long>>(member).Compile()().ToString();
                    case "Double":
                        return Expression.Lambda<Func<double>>(member).Compile()().ToString();
                    case "Decimal":
                        return Expression.Lambda<Func<decimal>>(member).Compile()().ToString();
                    case "String":
                        return "'" + Expression.Lambda<Func<string>>(member).Compile()() + "'";
                    case "DateTime":
                        return "'" + Expression.Lambda<Func<DateTime>>(member).Compile()() + "'";
                    default:
                        return Expression.Lambda<Func<object>>(member).Compile()().ToString();
                }

            }
            #endregion

            throw new Exception("无法解析该表达式：" + express);
        }

        /// <summary>
        /// 替换为sql的符号
        /// </summary>
        /// <param name="et"></param>
        /// <returns></returns>
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
                //case ExpressionType.Not:
                //    return "not";
                case ExpressionType.Convert:
                    return "";
                default:
                    throw new Exception("不支持" + et + "运算符查询");
            }
        }

        #endregion
    }
}
