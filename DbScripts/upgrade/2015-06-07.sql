--2015-06-07

/*
��Ҫ����
1�������Զ��弰��֣�Ĭ�����ֵܷ�60%��
2�������и����ο����Զ������ƣ�ԭ����Ĭ��Ϊ�Ծ�����

*/

--�����Ծ�ļ����
alter table TestPaper add Tp_PassScore int default 0 not null

--���ӿ����г��ε�����
sp_rename 'Examination.Exam_Name','Exam_Title','column'
go
alter table Examination add Th_ID int default 0 not null
go
alter table Examination add Th_Name  [nvarchar](255) NULL
go
alter table [Examination] add [Exam_Name] [nvarchar](255) NULL
go
alter table ExamResults add [Exam_Title] [nvarchar](255) NULL
go
alter table ExamResults add [Exr_SubmitTime] [datetime] NOT NULL
go
--���ӿ����е��ܷ֣����Ծ����й�����Ҳ���Բ�ͬ����
alter table Examination add Exam_Total int default 0 not null
go
alter table Examination add Exam_Tax int default 0 not null
go
--���Գɼ���������ѧԱ��Ϣ
sp_rename 'ExamResults.Acc_Id','St_ID','column'
go
sp_rename 'ExamResults.Acc_Name','St_Name','column'
go
