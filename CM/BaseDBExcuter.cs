using CMDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMDB
{
    /// <summary>
    /// 基础操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDBExcuter<T> where T : new()
    {
        #region 新增
        /// <summary>
        /// 单条记录添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T model)
        {
            return CM.DB.Insert<T>(model);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool Add(List<T> models)
        {
            return CM.DB.Insert<T>(models);
        }

        /// <summary>
        /// 添加一条返回主键
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object InsertReturnId(T model)
        {
            return CM.DB.InsertReturnId<T>(model);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 单条记录删除，根据Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Del(object Id)
        {
            return CM.DB.Delete<T>(Id);
        }

        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public bool Del(Expression<Func<T, bool>> express)
        {
            return CM.DB.Delete<T>(express);
        }
        #endregion

        #region 更新
        /// <summary> 
        /// 更新一条
        /// </summary> 
        public bool Update(T model)
        {
            return CM.DB.Update<T>(model);
        }

        /// <summary> 
        /// 批量更新
        /// </summary> 
        public bool Update(List<T> models)
        {
            return CM.DB.Update<T>(models);
        }
        #endregion

        #region 查询

        /// <summary>
        /// 根据主键查询一条
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetModel(object Id)
        {
            return CM.DB.Select<T>(Id);
        }

        /// <summary>
        /// 获取全部记录
        /// </summary>
        /// <returns></returns>
        public List<T> GetModels()
        {
            return CM.DB.Select<T>();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public List<T> GetModelsByLambda(Expression<Func<T, bool>> express)
        {
            return CM.DB.Select<T>(express);
        }
        #endregion
    }
}
