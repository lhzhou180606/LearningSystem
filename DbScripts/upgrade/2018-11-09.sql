--2018-11-9
/*���������еĴ��������*/
alter table Questions add Qus_Errornum int NULL
go
UPDATE Questions  SET Qus_Errornum = 0
GO
alter table Questions ALTER COLUMN Qus_Errornum [int] NOT NULL
go
/*�����½��Ƿ������ֶ�*/
alter table Outline add Ol_IsFinish bit NULL
go
UPDATE Outline  SET Ol_IsFinish = 1
GO
alter table Outline ALTER COLUMN Ol_IsFinish bit NOT NULL
go
/*���ӿγ���ʱ��ѵ��ֶ�*/
alter table Course add Cou_IsLimitFree bit NULL
go
UPDATE Course  SET Cou_IsLimitFree = 0
GO
alter table Course ALTER COLUMN Cou_IsLimitFree bit NOT NULL
go
alter table Course add Cou_FreeStart datetime NULL
go
UPDATE Course  SET Cou_FreeStart = GETDATE()
GO
alter table Course ALTER COLUMN Cou_FreeStart datetime NOT NULL
go
alter table Course add Cou_FreeEnd datetime NULL
go
UPDATE Course  SET Cou_FreeEnd = GETDATE()
GO
alter table Course ALTER COLUMN Cou_FreeEnd datetime NOT NULL
go
/*��Ƶ���Ƶ��ֶΣ�������3000�ַ�*/
alter table Accessory ALTER COLUMN  As_FileName nvarchar(3000);