/*����ʱ�Ƿ������л�����*/
alter table [Examination] add [Exam_IsToggle] [bit] NULL
go
update [Examination] set [Exam_IsToggle]=0
go
alter table [Examination] ALTER COLUMN [Exam_IsToggle] [bit] NOT NULL
go
/*�Ƿ���ʾȷ�ϰ�ť���а��գ���ȫ��Ϲ�ɵ���һ�Ű�ť��*/
alter table [Examination] add [Exam_IsShowBtn] [bit] NULL
go
update [Examination] set [Exam_IsShowBtn]=0
go
alter table [Examination] ALTER COLUMN [Exam_IsShowBtn] [bit] NOT NULL
go
/*�Ƿ��������Ҽ������ú��޷�����ճ��*/
alter table [Examination] add [Exam_IsRightClick] [bit] NULL
go
update [Examination] set [Exam_IsRightClick]=1
go
alter table [Examination] ALTER COLUMN [Exam_IsRightClick] [bit] NOT NULL
/*����ѧԱ���˺Ź���*/
go
alter table [Student] add Ac_ID [int] NULL
go
update [Student] set Ac_ID=0
go
alter table [Student] ALTER COLUMN Ac_ID [int] NOT NULL
go
alter table [Student] add [Ac_UID] [nvarchar](255) NULL
go