--�������б�����ɾ���ű�������������������
SELECT
    ('drop index ' + idx.name + ' on ' + OBJECT_NAME(CAST(idx.object_id AS INT)) + ';') AS dropIndexScript
FROM sys.tables tb
    INNER JOIN sys.indexes idx ON idx.object_id = tb.object_id
WHERE tb.type = 'u'
      AND idx.is_unique = 0
      AND idx.name IS NOT NULL
      
 
      
      
/*ɾ�����з���������*/
declare cursor_idx cursor scroll
for SELECT idx.name, OBJECT_NAME(CAST(idx.object_id AS INT)) as 'table' FROM sys.tables tb
    INNER JOIN sys.indexes idx ON idx.object_id = tb.object_id
	WHERE tb.type = 'u' AND idx.is_unique = 0 AND idx.name IS NOT NULL 
open cursor_idx
declare @idx nvarchar(500), @tb nvarchar(500),@sqlidx nvarchar(500)
fetch First from cursor_idx into @idx,@tb
while @@fetch_status=0  
 begin  
   set @sqlidx='drop index '+@idx+' on ' +@tb 
   --print @sqlidx
   exec sp_executesql @sqlidx
   fetch next from cursor_idx into  @idx,@tb
 end   
--�رղ��ͷ��α�
close cursor_idx
deallocate cursor_idx