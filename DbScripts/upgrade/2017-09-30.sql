--2017-09-30

/*���ӿ���ʱ������ͣ�1Ϊ�̶�ʱ�䣬2Ϊ����ʱ������*/
alter table [Examination] add Exam_DateType int NULL
go
update [Examination] set Exam_DateType=1
go
alter table [Examination] ALTER COLUMN Exam_DateType int NOT NULL
go
/*���ӽ���ʱ��*/
alter table [Examination] add Exam_DateOver [datetime] NULL
go
update [Examination] set Exam_DateOver=getdate() 
go
alter table [Examination] ALTER COLUMN Exam_DateOver [datetime] NOT NULL
go
/*����ѧԱ��ҵԺУ*/
alter table [Accounts] add [Ac_School] [nvarchar](255) NULL