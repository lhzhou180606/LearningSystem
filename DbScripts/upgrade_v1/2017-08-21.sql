--2017-08-21
/*�γ�����tax�����ͬ�Ĵ���*/
declare cur_couid cursor scroll
for select cou_id from Course order by cou_tax asc,Cou_CrtTime asc
open cur_couid
declare @couid int,@tax int
set @tax=0
fetch First from cur_couid into @couid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
   set @tax=@tax+1
   Update Course Set [Cou_tax]=@tax Where Cou_ID=@couid  --�޸ĵ�ǰ��
   --print @tax
   fetch next from cur_couid into @couid   --�ƶ��α�
 end   
--�رղ��ͷ��α�
close cur_couid
deallocate cur_couid