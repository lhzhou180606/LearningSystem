/*�޸��Ծ�����ֶγ���*/
alter table [TestPaper] ALTER COLUMN  [Tp_Intro] [nvarchar](max) NULL
go
/*�޸Ŀγ����ݵ��ֶγ���*/
alter table [Course] ALTER COLUMN  [Cou_Content] [nvarchar](max) NULL
go
/*��һЩntext�ֶΣ��ĳ�nvarchar max*/
alter table [Subject] ALTER COLUMN  [Sbj_Intro] [nvarchar](max) NULL
go
alter table [Article] ALTER COLUMN  [Art_Intro] [nvarchar](max) NULL
go
alter table [Article] ALTER COLUMN  [Art_Details] [nvarchar](max) NULL
go
alter table [Article] ALTER COLUMN  [Art_Endnote] [nvarchar](max) NULL
go