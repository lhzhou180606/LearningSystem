/*ѧԱѡ�޿γ̵ļ�¼������ѧϰ��������Ϣ�ļ�¼*/
alter table [Student_Course] add Lc_Code nvarchar(100)
go
alter table [Student_Course] add Lc_Pw nvarchar(50)
go
/*��Ƶѧϰ��¼��������*/
alter table [LogForStudentStudy] add Lss_Details [nvarchar](max) NULL
go
/*�½����ӱ༭ʱ��*/
alter table [Outline] add Ol_ModifyTime datetime null
go
update [Outline] set Ol_ModifyTime=GETDATE()
go
alter table [Outline] ALTER COLUMN Ol_ModifyTime datetime NOT NULL
/*�½����Ƿ����*/
alter table [Outline] add Ol_IsChecked bit null
go
update [Outline] set Ol_IsChecked=1
go
alter table [Outline] ALTER COLUMN Ol_IsChecked bit NOT NULL
