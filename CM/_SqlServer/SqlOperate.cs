using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace CMDB
{
    public class SqlOperate : DBAbstract
    {
        public SqlOperate(string _Constring)
        {
            Constring = _Constring;
        }


        ICreateSql ics;

        #region 查询
        /// <summary>
        /// 无参
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override List<T> Select<T>()
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new SelectSql();
                using (IDataReader idr = SqlTools.GetDtReader(ics.CreateSql<T>(), sct))
                {
                    return SqlResult.ValueToList<T>(idr);
                }
            }
        }

        //根据主键查询
        public override T Select<T>(object Value)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new SelectSql();
                using (IDataReader reader = SqlTools.GetDtReader(ics.CreateSql<T>(Value), sct))
                {
                    return SqlResult.ValueToModel<T>(reader);
                }

            }
        }


        public override List<T> Select<T>(Expression<Func<T, bool>> express)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                SelectSql cs = new SelectSql();
                using (IDataReader idr = SqlTools.GetDtReader(cs.CreateSql<T>(express), sct))
                {

                    return SqlResult.ValueToList<T>(idr);
                }
            }
        }
        #endregion

        #region 插入
        public override int Insert<T>(T t)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new InsertSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(t), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }

        public override object InsertReturnId<T>(T t)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                string ISql = InsertSql.CreateSqlId<T>(t);//生成查询ID的SQL
                using (SqlCommand scmd = new SqlCommand(InsertSql.CreateSqlId<T>(t), sct))
                {
                    return SqlResult.returnObject(scmd);
                }
            }
        }

        public override int Insert<T>(List<T> t)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new InsertSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(t), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }
        #endregion

        #region 更新
        public override int Update<T>(T t)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new UpdateSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(t), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }

        public override int Update<T>(List<T> t)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new UpdateSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(t), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }
        #endregion

        #region 删除
        public override int Delete<T>()
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new DeleteSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }

        public override int Delete<T>(object Value)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new DeleteSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(Value), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }

        public override int Delete<T>(T t)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new DeleteSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(t), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }

        public override int Delete<T>(List<T> t)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new DeleteSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(t), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }
        public override int Delete<T>(Expression<Func<T, bool>> express)
        {
            using (SqlConnection sct = new SqlConnection(Constring))
            {
                sct.Open();
                ics = new DeleteSql();
                using (SqlCommand scmd = new SqlCommand(ics.CreateSql<T>(express), sct))
                {
                    return SqlResult.Result(scmd);
                }
            }
        }
        #endregion
    }
}