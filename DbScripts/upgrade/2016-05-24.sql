

/*������ʦ�Ƿ���ʾ���ֶΣ����Կ��ƽ�ʦ�Ƿ���ǰ̨չʾ*/
alter table [Teacher] add Th_IsShow [bit] NULL
go
update [Teacher] set Th_IsShow=1
go
alter table [Teacher] ALTER COLUMN Th_IsShow [bit] NOT NULL