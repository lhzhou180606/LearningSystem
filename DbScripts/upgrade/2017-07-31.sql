--2017-07-30
/*���Ӹ����ĵ���������*/
alter table [Accessory] add [As_IsOther] [bit] NULL
go
update [Accessory] set [As_IsOther]=0
go
alter table [Accessory] ALTER COLUMN [As_IsOther] [bit] NOT NULL