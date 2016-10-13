using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;

namespace CMDB
{
    /// <summary>
    /// EF帮助类
    /// </summary>
    /// <typeparam name="TContext">上下文对象</typeparam>
    /// <typeparam name="TEntity">T</typeparam>
    public class BaseEfExcuter<TContext, TEntity>
        where TContext : DbContext, new()
        where TEntity : class, new()
    {
        #region 增

        /// <summary>
        /// 新增一条
        /// </summary>
        /// <param name="entity">要插入的Model</param>
        /// <returns></returns>
        public bool Add(TEntity entity)
        {
            using (DbContext context = new TContext())
            {
                context.Entry<TEntity>(entity).State = EntityState.Added;
                return context.SaveChanges() > 0;

            }
        }

        /// <summary>
        /// 返回当前新增的记录
        /// </summary>
        /// <param name="entity">要插入的Model</param>
        /// <returns></returns>
        public TEntity AddReturnT(TEntity entity)
        {
            using (DbContext context = new TContext())
            {
                context.Set<TEntity>().Add(entity);
                if (context.SaveChanges() > 0)
                    return entity;
                else
                    return default(TEntity);
            }
        }
        #endregion

        #region 删

        /// <summary>
        /// 删除单条（对应主键删除）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Delete(TEntity entity)
        {
            using (DbContext context = new TContext())
            {
                context.Set<TEntity>().Attach(entity);
                context.Entry<TEntity>(entity).State = EntityState.Deleted;
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="expression">Lambda</param>
        /// <returns></returns>
        public int Delete(Expression<Func<TEntity, bool>> expression)
        {
            using (DbContext context = new TContext())
            {
                foreach (TEntity item in GetModels(expression))
                {
                    context.Set<TEntity>().Attach(item);
                    context.Entry<TEntity>(item).State = EntityState.Deleted;
                }

                return context.SaveChanges();
            }
        }

        #endregion

        #region 改

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(TEntity entity)
        {
            using (DbContext context = new TContext())
            {
                if (context.Entry<TEntity>(entity).State == EntityState.Detached)
                {
                    context.Set<TEntity>().Attach(entity);
                    context.Entry<TEntity>(entity).State = EntityState.Modified;
                }
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// 批量更新,目前只能更新单个字段Action问题
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public int Updates(Expression<Func<TEntity, bool>> expression, Action<TEntity> updateExpression)//Expression<Func<TEntity, TEntity>> updateExpression
        {
            using (DbContext context = new TContext())
            {
                List<TEntity> list = GetModels(expression);
                list.ForEach(updateExpression);
                foreach (TEntity item in list)
                {
                    context.Set<TEntity>().Attach(item);
                    context.Entry<TEntity>(item).State = EntityState.Modified;
                }
                return context.SaveChanges();
            }
        }


        #endregion

        #region 查

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TEntity GetModel(Expression<Func<TEntity, bool>> expression)
        {
            using (DbContext context = new TContext())
            {
                return context.Set<TEntity>().Where(expression).FirstOrDefault();
            }
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetModels()
        {
            using (DbContext context = new TContext())
            {
                return context.Set<TEntity>().ToList();
            }
        }

        /// <summary>
        /// 查询全部（分页）
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="total">总条数</param>
        /// <param name="IsDesc">是否降序</param>
        /// <param name="orderLambda">排序字段</param>
        /// <returns></returns>
        public List<TEntity> GetModels<S>(int pageIndex, int pageSize, out int total, bool IsDesc, Func<TEntity, S> orderLambda)
        {
            using (DbContext context = new TContext())
            {
                var data = context.Set<TEntity>();
                List<TEntity> list = new List<TEntity>();
                total = data.Count();
                if (IsDesc)
                    list = data.OrderByDescending(orderLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                else
                    list = data.OrderBy(orderLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                return list;
            }
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<TEntity> GetModels(Expression<Func<TEntity, bool>> expression)
        {
            using (DbContext context = new TContext())
            {
                return context.Set<TEntity>().Where(expression).ToList();
            }
        }

        /// <summary>
        /// 分根据条件查询（分页）
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="expression">查询条件</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="total">总条数</param>
        /// <param name="IsDesc">是否降序</param>
        /// <param name="orderLambda">排序字段</param>
        /// <returns></returns>
        public List<TEntity> GetModels<S>(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, out int total, bool IsDesc, Func<TEntity, S> orderLambda)
        {
            using (DbContext context = new TContext())
            {
                var data = context.Set<TEntity>().Where(expression);
                List<TEntity> list = new List<TEntity>();
                total = data.Count();
                if (IsDesc)
                    list = data.OrderByDescending(orderLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                else
                    list = data.OrderBy(orderLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList(); ;
                return list;
            }
        }

        #endregion

    }
}
