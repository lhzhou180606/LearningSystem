/*���ӿ��Գɼ��У��Ƿ������ֶ�*/
alter table [ExamResults] add Exr_IsCalc [bit] NULL
go
update [ExamResults] set Exr_IsCalc=0
go
alter table [ExamResults] ALTER COLUMN Exr_IsCalc [bit] NOT NULL