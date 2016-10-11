using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CMDB
{
    public interface IDataBase
    {
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回所有数据</returns>
        List<T> Select<T>() where T : new();

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T Select<T>(object Value) where T : new();

        /// <summary>
        /// 根据条件查询当前实体数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="express">条件</param>
        /// <returns></returns>
        List<T> Select<T>(Expression<Func<T, bool>> express) where T : new();



        /// <summary>
        /// 插入一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns>是否成功</returns>
        bool Insert<T>(T t);

        /// <summary>
        ///插入一条数据，并返回插入数据的自增主键（ID）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        object InsertReturnId<T>(T t);

        /// <summary>
        /// 插入多条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">实体数组</param>
        /// <returns>是否插入成功</returns>
        bool Insert<T>(List<T> t);



        /// <summary>
        /// 更新一条数据（只更新有数据，无数据(null)的不变）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Update<T>(T t);

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">实体数组</param>
        /// <returns></returns>
        bool Update<T>(List<T> t);



        /// <summary>
        /// 清空表，慎用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Delete<T>();

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value">主键值</param>
        /// <returns></returns>
        bool Delete<T>(object Value);

        /// <summary>
        /// 删除一条,Model中有主键即可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Delete<T>(T t);

        /// <summary>
        /// 删除多条,Model中有主键即可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">实体数组</param>
        /// <returns></returns>
        bool Delete<T>(List<T> t);

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="express">条件</param>
        /// <returns></returns>
        bool Delete<T>(Expression<Func<T, bool>> express) where T : new();

    }
}
