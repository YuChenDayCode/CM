using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CMDB
{
    /// <summary>
    /// 生成SQL接口
    /// </summary>
    public interface ICreateSql
    {
        /// <summary>
        /// 操作所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string CreateSql<T>();


        /// <summary>
        /// 根据主键生成SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        string CreateSql<T>(object obj);

        string CreateSql<T>(T t);

        string CreateSql<T>(List<T> t);

        /// <summary>
        /// 根据Lambda生成SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="express"></param>
        /// <returns></returns>
        string CreateSql<T>(Expression<Func<T, bool>> express);
    }
}
