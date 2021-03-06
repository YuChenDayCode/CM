﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace CMDB
{
    /// <summary>
    /// 控制返回类
    /// </summary>
    public class SqlResult
    {
        #region 查询返回处理
        /// <summary>
        /// 数据转化为MODEL
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="_counts">当行总条数</param>
        /// <param name="type">Type对象</param>
        /// <param name="reader">IDataReader</param>
        /// <param name="model">MODEL</param>
        public static void ValueToModel<T>(int _counts, Type type, IDataReader reader, T model)
        {
            for (int i = 0; i < _counts; i++)
            {
                string fieldname = reader.GetName(i);
                object value = reader[i];
                if (value != null && !Convert.IsDBNull(value))
                {
                    PropertyInfo pi = type.GetProperty(fieldname);
                    if (pi != null)
                        pi.SetValue(model, value, null);
                }
            }
        }
        /// <summary>
        /// 单条数据返会model
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="reader">IDataReader对象</param>
        public static T ValueToModel<T>(IDataReader reader)
        {
            bool IsValue = true;//是否有值
            Type type = typeof(T);
            T model = (T)Activator.CreateInstance(type);
            while (reader.Read())
            {
                ValueToModel<T>(reader.FieldCount, type, reader, model);
                IsValue = false;
            }
            if (IsValue) return default(T);
            return model;
        }
        /// <summary>
        /// 数据转化为List
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="reader">IDataReader对象</param>
        public static List<T> ValueToList<T>(IDataReader reader)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            T model = (T)Activator.CreateInstance(type);
            while (reader.Read())
            {
                ValueToModel<T>(reader.FieldCount, type, reader, model);
                list.Add(model);
                model = (T)Activator.CreateInstance(model.GetType());//重新创建实体对象
            }
            return list;
        }

        /// <summary>
        /// 返回单个字段
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static object returnObject(SqlCommand cmd)
        {
            return cmd.ExecuteScalar();
        }
        #endregion

        #region 插入、更新、删除返回处理
        /// <summary>
        /// 插入，删除更新单条
        /// </summary>
        /// <param name="ocm"></param>
        /// <returns></returns>
        public static int Result(SqlCommand cmd)
        {
            int i = cmd.ExecuteNonQuery();
            return i;
        }

        #endregion
    }
}
