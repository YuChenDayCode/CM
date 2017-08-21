# CM
## ORM

 * 支持Oracle、SqlServer数据库
       根据配置文件识别
        <pre><code>```
	<add key="ConStr" value="Data Source=.;Initial Catalog=DBName;User ID= ;PassWord= " />
	<add key="ConType" value="Sql"/> ```
	</pre></code>
	 

* 查询Select()：查询所有，根据主键、lambda条件查询(数组.Contains()=in,字符串.Contains()=模糊查询)
* 插入Insert()：插入一条、多条
* 插入一条返回主键 InsertReturnId()
* 更新Update()：更新一条、多条
* 删除Delete()：清空表，删除、删除多条，根据主键、Linq条件删除
* CM.DB.Select<Test>(t => t.Content.StartsWith("1"));//模糊匹配，以该字符串开头的 
* CM.DB.Select<Test>(t => t.Content.EndsWith("2")); //模糊匹配，以该字符串结尾的
* CM.DB.Select<Test>(t => t.Content.Contains("1")); //模糊匹配，任意位置包含该字符串的 
<br> 

        
    
#### 调用：
     入口类CM调用接口DB
	 对应MODEL需引用using CMDB;的名称空间 并在主键上加入 [Col_(IsPrimaryKey = true, IsIdentity = true)] 

## 所有数字类型皆需设置为可为空，方便更新 
## 请注意每个方法都是泛型方法，T类型需与表名相同




# [我的博客](http://blog.csdn.net/commandbaby "CSDN")  
