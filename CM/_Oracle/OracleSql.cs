using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Linq.Expressions;

namespace CMDB
{
    /// <summary>
    /// 创建SQL的公共类
    /// </summary>
    public class OracleSql
    {
        public static IDataReader GetDtReader(string sql, OracleConnection oct)
        {
            using (OracleCommand ocm = new OracleCommand(sql, oct))
            {
                return ocm.ExecuteReader();
            }
        }
    }


    public class SelectOql : ICreateSql
    {
        public string CreateSql<T>()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from ");
            sb.Append(typeof(T).Name);
            return sb.ToString();
        }

        public string CreateSql<T>(object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from ");
            sb.Append(typeof(T).Name);
            sb.Append(" where ");
            sb.Append(CommonFunc.GetSql<T>(obj));
            return sb.ToString();
        }

        public string CreateSql<T>(T t)
        {
            throw new NotImplementedException();
        }

        public string CreateSql<T>(List<T> t)
        {
            throw new NotImplementedException();
        }

        public string CreateSql<T>(Expression<Func<T, bool>> express)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from ");
            sb.Append(typeof(T).Name);
            sb.Append(" where ");
            sb.Append(CommonFunc.Resolve(express.Body));
            return sb.ToString(); ;
        }

    }

    public class InsertOql : ICreateSql
    {
        public string CreateSql<T>()
        {
            throw new NotImplementedException();
        }

        public string CreateSql<T>(Expression<Func<T, bool>> express)
        {
            throw new NotImplementedException();
        }

        public string CreateSql<T>(object obj)
        {
            throw new NotImplementedException();
        }

        public string CreateSql<T>(T t)
        {
            Type type = typeof(T);
            string tableName = type.Name;
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_v = new StringBuilder();
            sb.Append("insert into ");
            sb.Append(tableName);
            sb.Append("(");

            sb_v.Append(" values (");
            foreach (var pi in type.GetProperties())
            {
                if (pi != null)
                {
                    sb.Append(pi.Name + ",");
                    object value = pi.GetValue(t);
                    value = CommonFunc.UnInjection(value);
                    if (value != null &&
                        (value.GetType() == typeof(int) || value.GetType() == typeof(decimal) || value.GetType() == typeof(double) || value.GetType() == typeof(long) || value.GetType() == typeof(float)))
                        sb_v.Append(value + ",");
                    else
                        sb_v.Append("'" + value + "',");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");

            sb_v.Remove(sb_v.Length - 1, 1);
            sb_v.Append(")");

            sb.Append(sb_v);

            return sb.ToString();
        }

        public string CreateSql<T>(List<T> t)
        {
            return CommonFunc.GetSql<T>(t, this);
        }


    }

    public class UpdateOql : ICreateSql
    {
        public string CreateSql<T>()
        {
            throw new NotImplementedException();
        }

        public string CreateSql<T>(Expression<Func<T, bool>> express)
        {
            throw new NotImplementedException();
        }

        public string CreateSql<T>(object obj)
        {
            throw new NotImplementedException();
        }

        public string CreateSql<T>(T t)
        {
            string PrimaryName = "";//主键名、值
            object ParmaryValue = null;

            Type type = typeof(T);
            string tableName = type.Name;


            StringBuilder sb = new StringBuilder();
            StringBuilder sb_u = new StringBuilder();
            sb.Append("update ");
            sb.Append(tableName);
            sb.Append(" set ");

            foreach (var pi in type.GetProperties())
            {
                if (pi != null)
                {
                    object value = pi.GetValue(t);//字段对应的值
                    #region 主键
                    object[] obj = pi.GetCustomAttributes(false);//取特性
                    if (obj.Count() > 0)
                    {
                        Col_Attribute ca = obj[0] as Col_Attribute;
                        if (ca.IsPrimaryKey)//主键
                        {
                            PrimaryName = pi.Name;
                            ParmaryValue = value;
                            continue;
                        }
                    }
                    #endregion
                    if (value == null) continue; //如果该字段为null不更新
                    value = CommonFunc.UnInjection(value);
                    sb_u.Append(pi.Name + "=");
                    if (value != null &&
                        (value.GetType() == typeof(int) || value.GetType() == typeof(decimal) || value.GetType() == typeof(double) || value.GetType() == typeof(long) || value.GetType() == typeof(float)))
                        sb_u.Append(value + ",");
                    else
                        sb_u.Append("'" + value + "',");
                }
            }

            if (string.IsNullOrEmpty(PrimaryName))
                throw new Exception("需要主键(请检查表是否设置主键以及MODEL主键字段是否设置特性)");
            else if (ParmaryValue == null)
                throw new Exception("请检查主键值");
            else if (string.IsNullOrEmpty(sb_u.ToString()))
                throw new Exception("无更新");
            else
            {
                sb_u.Remove(sb_u.Length - 1, 1);
                sb.Append(sb_u);
                sb.Append(" where ");
                sb.Append(PrimaryName + " = " + ParmaryValue);
            }

            return sb.ToString();
        }

        public string CreateSql<T>(List<T> t)
        {
            return CommonFunc.GetSql<T>(t, this);
        }
    }

    public class DeleteOql : ICreateSql
    {
        public string CreateSql<T>()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete from ");
            sb.Append(typeof(T).Name);
            return sb.ToString();
        }

        public string CreateSql<T>(Expression<Func<T, bool>> express)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete from ");
            sb.Append(typeof(T).Name);
            sb.Append(" where ");
            sb.Append(CommonFunc.Resolve(express.Body));
            return sb.ToString();
        }

        public string CreateSql<T>(object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete from ");
            sb.Append(typeof(T).Name);
            sb.Append(" where ");
            sb.Append(CommonFunc.GetSql<T>(obj));
            return sb.ToString();
        }

        public string CreateSql<T>(T t)
        {
            string PrimaryName = "";//主键名、值
            object ParmaryValue = null;

            Type type = typeof(T);
            string tableName = type.Name;

            StringBuilder sb = new StringBuilder();
            sb.Append("delete ");
            sb.Append(tableName);
            sb.Append(" where ");

            foreach (var pi in type.GetProperties())
            {
                if (pi != null)
                {
                    object value = pi.GetValue(t);//字段对应的值

                    object[] obj = pi.GetCustomAttributes(false);
                    if (obj.Count() > 0)
                    {
                        Col_Attribute ca = obj[0] as Col_Attribute;
                        if (ca.IsPrimaryKey)//主键
                        {
                            PrimaryName = pi.Name;
                            ParmaryValue = value;
                            break;
                        }
                    }
                }
            }
            ParmaryValue = CommonFunc.UnInjection(ParmaryValue);
            if (string.IsNullOrEmpty(PrimaryName))
                throw new Exception("需要主键(请检查表是否设置主键以及MODEL主键字段是否设置特性)");
            else if (ParmaryValue == null)
                throw new Exception("请检查主键值");
            else
            {
                sb.Append(PrimaryName + "=" + ParmaryValue);
            }

            return sb.ToString();
        }

        public string CreateSql<T>(List<T> t)
        {
            return CommonFunc.GetSql<T>(t, this);
        }

    }

}
