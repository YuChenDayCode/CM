# CM
CMDB/ORM

 * ֧��Oracle��SqlServer���ݿ�
       ���������ļ�ʶ��
       --<add key="ConStr" value="Data Source=.;Initial Catalog=TestDB;User ID=sa;PassWord=123456" /> <add key="ConType" value="Sql"/>

     ���ѯSelect()����ѯ���У�����������Linq������ѯ(����.Contains()=in,�ַ���.Contains()=ģ����ѯ)
     �����Insert()������һ��������
     �����һ���������� InsertReturnId()
     �����Update()������һ��������
     ��ɾ��Delete()����ձ�ɾ����ɾ������������������Linq����ɾ��
     ����ע��ÿ���������Ƿ��ͷ�����T�������������ͬ
        

 * Linq֧�ֵ�һԪ���η�����
     CM.DB.Select<Test>(t => t.Content.StartsWith("1"));//ģ��ƥ�䣬�Ը��ַ�����ͷ��
     CM.DB.Select<Test>(t => t.Content.EndsWith("2")); //ģ��ƥ�䣬�Ը��ַ�����β��
     CM.DB.Select<Test>(t => t.Content.Contains("1")); //ģ��ƥ�䣬����λ�ð������ַ�����

 *���ã�
     �����CM���ýӿ�DB
	 ��ӦMODEL������using CMDB;�����ƿռ� ���������ϼ��� [Col_(IsPrimaryKey = true, IsIdentity = true)] 

�����������ͽ�������Ϊ��Ϊ�գ��������