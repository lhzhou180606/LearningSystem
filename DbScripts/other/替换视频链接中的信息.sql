
 /*�滻��Ƶ���ӵ�ַ�е���Ϣ*/
declare @old nvarchar(max),@new nvarchar(max),@sql nvarchar(max)
set @old='��Ƶ��ַ'		--ԭ�ַ���
set @new='����Ƶ��ַ'	--���ַ���
set @sql='update Accessory set as_filename= replace(cast(as_filename as nvarchar(100)), '''+@old+''', '''+@new+''') where (as_type=''CourseVideo'' and as_isouter=1 and as_isother=0) and as_filename like ''%'+@old+'%'''
print @sql
exec(@sql);
 