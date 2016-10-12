using System;



//2016-10-12 已将项目提交到github

namespace CMDB
{
    /// <summary>
    /// ORM框架 支持Oracle、SqlServer数据库
    /// </summary>
    public class CM
    {
        private static IDataBase idb;

        static CM() //静态构造函数，在执行完之后才回执行其他函数
        {
            string ConStr = System.Configuration.ConfigurationManager.AppSettings["ConStr"];//连接字符串
            if (string.IsNullOrEmpty(ConStr))
                throw new Exception("请在配置文件中添加节点ConStr，值为连接字符串");
            string ConType = System.Configuration.ConfigurationManager.AppSettings["ConType"];//连接类
            if (string.IsNullOrEmpty(ConType))
                throw new Exception("请在配置文件中添加节点ConType，值为数据了类型，Sql或Oracle");

            ConType = "CMDB." + ConType + "Operate";
            object[] para = { ConStr };
            idb = (IDataBase)Activator.CreateInstance(Type.GetType(ConType), para);
        }

        ///<summary>
        ///<para>方法：</para>
        ///<para>●查询Select()：查询所有，根据主键、Linq条件查询</para>
        ///<para>●插入Insert()：插入一条、多条</para>
        ///<para>●插入一条返回主键 InsertReturnId()</para>
        ///<para>●更新Update()：更新一条、多条</para>
        ///<para>●删除Delete()：清空表，删除、删除多条，根据主键、Linq条件删除</para>
        ///<para>●[请注意每个方法都是泛型方法，类型需与表名相同]</para>
        ///</summary>
        public static IDataBase DB
        {
            get { return idb; }
            set { idb = value; }
        }
    }
}
