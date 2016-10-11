using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq.Expressions;

namespace CMDB
{
    public class OracleOperate : DBAbstract
    {
        public OracleOperate(string _Constring)
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
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new SelectOql();
                using (IDataReader idr = OracleSql.GetDtReader(ics.CreateSql<T>(), oct))
                {
                    return OracleResult.ValueToList<T>(idr);
                }
            }
        }

        //根据主键查询
        public override T Select<T>(object Value)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new SelectOql();
                using (IDataReader reader = OracleSql.GetDtReader(ics.CreateSql<T>(Value), oct))
                {
                    return OracleResult.ValueToModel<T>(reader);
                }

            }
        }


        public override List<T> Select<T>(Expression<Func<T, bool>> express)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                SelectOql cs = new SelectOql();
                using (IDataReader idr = OracleSql.GetDtReader(cs.CreateSql<T>(express), oct))
                {

                    return OracleResult.ValueToList<T>(idr);
                }
            }
        }
        #endregion

        #region 插入
        public override bool Insert<T>(T t)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new InsertOql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(t), oct))
                {
                    return OracleResult.SingleResult(ocm);
                }
            }
        }

        public override object InsertReturnId<T>(T t)
        {
            throw new NotImplementedException("Oracle不支持该方法。");
        }

        public override bool Insert<T>(List<T> t)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new InsertOql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(t), oct))
                {
                    return OracleResult.ManyResult(ocm);
                }
            }
        }
        #endregion

        #region 更新
        public override bool Update<T>(T t)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new UpdateOql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(t), oct))
                {
                    return OracleResult.SingleResult(ocm);
                }
            }
        }

        public override bool Update<T>(List<T> t)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new UpdateOql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(t), oct))
                {
                    return OracleResult.ManyResult(ocm);
                }
            }
        }
        #endregion

        #region 删除
        public override bool Delete<T>()
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new DeleteSql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(), oct))
                {
                    return OracleResult.SingleResult(ocm);
                }
            }
        }

        public override bool Delete<T>(object Value)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new DeleteOql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(Value), oct))
                {
                    return OracleResult.SingleResult(ocm);
                }
            }
        }

        public override bool Delete<T>(T t)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new DeleteOql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(t), oct))
                {
                    return OracleResult.SingleResult(ocm);
                }
            }
        }

        public override bool Delete<T>(List<T> t)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new DeleteOql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(t), oct))
                {
                    return OracleResult.ManyResult(ocm);
                }
            }
        }
        public override bool Delete<T>(Expression<Func<T, bool>> express)
        {
            using (OracleConnection oct = new OracleConnection(Constring))
            {
                oct.Open();
                ics = new DeleteOql();
                using (OracleCommand ocm = new OracleCommand(ics.CreateSql<T>(express), oct))
                {
                    return OracleResult.SingleResult(ocm);
                }
            }
        }

        #endregion
    }
}