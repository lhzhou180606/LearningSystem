/*�޸ĵ����¼�����������״̬�ֶ�Ϊ����Ϊ��*/
alter table SingleSignOn ALTER COLUMN 
SSO_IsUse bit NOT NULL
go
/*�����¼����������Ƴ������ó�һЩ*/
alter table SingleSignOn ALTER COLUMN 
SSO_Name nvarchar(100)
go
/*�����¼�����������Ƿ�������ѧԱ��*/
alter table SingleSignOn add SSO_IsAddSort [bit] default 0 NULL
go



