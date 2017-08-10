using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CMDB
{
    public abstract class DBAbstract : IDataBase
    {
        public static string Constring { get; set; }


        public abstract List<T> Select<T>() where T : new();

        public abstract List<T> Select<T>(string sql) where T : new();

        public abstract T Select<T>(object Value) where T : new();

        public abstract List<T> Select<T>(Expression<Func<T, bool>> express) where T : new();

        public abstract object Select(string sql);


        public abstract int Insert<T>(T t);

        public abstract object InsertReturnId<T>(T t);

        public abstract int Insert<T>(List<T> t);



        public abstract int Update<T>(T t);

        public abstract int Update(string sql);

        public abstract int Update<T>(List<T> t);



        public abstract int Delete<T>();

        public abstract int Delete<T>(object Value);

        public abstract int Delete<T>(T t);

        public abstract int Delete<T>(List<T> t);

        public abstract int Delete<T>(Expression<Func<T, bool>> express) where T : new();

       
    }
}
