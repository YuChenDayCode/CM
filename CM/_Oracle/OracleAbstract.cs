using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMDB;
using System.Linq.Expressions;

namespace CMDB
{
    public abstract class OracleAbstract : IDataBase
    {
        public static string Constring { get; set; }


        public abstract List<T> Select<T>() where T : new();

        public abstract T Select<T>(object Value) where T : new();

        public abstract List<T> Select<T>(Expression<Func<T, bool>> express) where T : new();


        public abstract bool Insert<T>(T t);

        public abstract bool Insert<T>(List<T> t);



        public abstract bool Update<T>(T t);

        public abstract bool Update<T>(List<T> t);



        public abstract bool Delete<T>();

        public abstract bool Delete<T>(object Value);

        public abstract bool Delete<T>(T t);

        public abstract bool Delete<T>(List<T> t);

        public abstract bool Delete<T>(Expression<Func<T, bool>> express) where T : new();
    }
}
