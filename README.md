# CM
## CMDB/ORM

 * ֧��Oracle��SqlServer���ݿ�
       ���������ļ�ʶ��
        <pre><code>```
	<add key="ConStr" value="Data Source=.;Initial Catalog=DBName;User ID= ;PassWord= " />
	<add key="ConType" value="Sql"/> ```
	</pre></code>
	 

* ��ѯSelect()����ѯ���У�����������Linq������ѯ(����.Contains()=in,�ַ���.Contains()=ģ����ѯ)
* ����Insert()������һ��������<
* ����һ���������� InsertReturnId()
* ����Update()������һ��������
* ɾ��Delete()����ձ�ɾ����ɾ������������������Linq����ɾ��
* CM.DB.Select<Test>(t => t.Content.StartsWith("1"));//ģ��ƥ�䣬�Ը��ַ�����ͷ�� 
* CM.DB.Select<Test>(t => t.Content.EndsWith("2")); //ģ��ƥ�䣬�Ը��ַ�����β��
* CM.DB.Select<Test>(t => t.Content.Contains("1")); //ģ��ƥ�䣬����λ�ð������ַ����� 
<br> 

        
    
#### ���ã�
     �����CM���ýӿ�DB
	 ��ӦMODEL������using CMDB;�����ƿռ� ���������ϼ��� [Col_(IsPrimaryKey = true, IsIdentity = true)] 

## �����������ͽ�������Ϊ��Ϊ�գ�������� 
## ��ע��ÿ���������Ƿ��ͷ�����T�������������ͬ




# [�ҵĲ���](http://write.blog.csdn.net/postlist "CSDN")  