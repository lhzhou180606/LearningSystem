--2018-06-25

/*����֧���ӿ��е�Ӧ�ó�����*/
alter table [PayInterface] add [Pai_Scene] [nvarchar](500) NULL
go
UPDATE [PayInterface]   SET [Pai_Scene] = 'alipay,h5' WHERE [Pai_Pattern]='֧�����ֻ�֧��'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'alipay,native' WHERE [Pai_Pattern]='֧������ҳֱ��'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'weixin,public' WHERE [Pai_Pattern]='΢�Ź��ں�֧��'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'weixin,native' WHERE [Pai_Pattern]='΢��ɨ��֧��'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'weixin,mini' WHERE [Pai_Pattern]='΢��С����֧��'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'weixin,h5' WHERE [Pai_Pattern]='΢��Html5֧��'
GO