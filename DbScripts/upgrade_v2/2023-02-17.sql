/*ѧԱ��������ѧԱ����*/
alter table [StudentSort] add [Sts_Count] [int] default 0 NOT NULL
go
declare cursor_acc cursor scroll
for select Sts_ID,COUNT(*) as 'count' from Accounts group by Sts_ID
open cursor_acc
declare @stsid bigint, @count int
fetch First from cursor_acc into @stsid,@count
while @@fetch_status=0  
 begin  
   update [StudentSort] set [Sts_Count]=@count where Sts_ID=@stsid
   fetch next from cursor_acc into @stsid,@count
 end   
--�رղ��ͷ��α�
close cursor_acc
deallocate cursor_acc