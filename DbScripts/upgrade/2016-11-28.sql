-2016-11-28
/*
�������ݣ�
1���Ծ�������⣬���԰��½ڳ��⣻
2�������ȫ����£������µ���Ʒ��
3���Ż�ģ�����棬�򻯱��ֲ���룻
4����������bug��
*/
/*�����Ծ��ע�������ֶ�*/
alter table [TestPaper] add Tp_Remind [nvarchar](max) NULL
go
/*�����Ծ�ĸ������ֶ�*/
alter table [TestPaper] add Tp_SubName [nvarchar](255) NULL
go
/*ԭ�Ծ�������ֶγ������ӵ�255*/
alter table [TestPaper] ALTER COLUMN [Tp_Name] [nvarchar](255) NULL
go
/*�Ծ������ѡ��Χ��Ϊ0ʱ���γ�ѡ�⣬Ϊ1ʱ���½�ѡ��*/
alter table [TestPaper]  add Tp_FromType [int] NULL
go
update [TestPaper]  set Tp_FromType=0
go
alter table [TestPaper]  ALTER COLUMN Tp_FromType [int] NOT NULL
go
alter table [TestPaper] add Tp_FromConfig [nvarchar](max) NULL
go
/*�Ծ��趨������������½�*/
alter table [TestPagerItem]  add Ol_ID [int] NULL
go
update [TestPagerItem]  set Ol_ID=0
go
alter table [TestPagerItem]  ALTER COLUMN Ol_ID [int] NOT NULL
go
/*ѧԱ�ֶ��е�ʱ�䣬����Ϊ����Ϊ��*/
update [Student]  set [St_CrtTime]=getdate() where [St_CrtTime] is null
go
alter table [Student]  ALTER COLUMN [St_CrtTime] [datetime] not NULL
go
/*����ƴ���ˣ���һ��*/
EXEC sp_rename 'TestPagerItem', 'TestPaperItem';
/*����һЩ�ֶβ���Ϊ��*/
update AddressList  set Ads_Id=0 where Ads_Id is null
go
alter table AddressList  ALTER COLUMN Ads_Id [int] NOT NULL
go
update AddressList  set Adl_Sex=0 where Adl_Sex is null
go
alter table AddressList  ALTER COLUMN Adl_Sex [int] NOT NULL
go
update AddressList  set Acc_Id=0 where Acc_Id is null
go
alter table AddressList  ALTER COLUMN Acc_Id [int] NOT NULL
go
update AddressList  set Adl_State=0 where Adl_State is null
go
alter table AddressList  ALTER COLUMN Adl_State [int] NOT NULL
go
update AddressList  set Adl_Birthday=dateadd(day,2,'1770-01-01') where Adl_State is null
go
alter table AddressList  ALTER COLUMN Adl_Birthday [datetime] NOT NULL
go
update AddressList  set Adl_CrtTime=dateadd(day,2,'1770-01-01') where Adl_State is null
go
alter table AddressList  ALTER COLUMN Adl_CrtTime [datetime] NOT NULL
go
