/*ɾ��һЩѧԱ�Ѿ������ڣ���ѧϰ��¼��������ѧϰ��¼������*/
declare @num int
set @num=0
declare cur_acc cursor scroll
for select Ac_ID  from LogForStudentStudy group by Ac_ID
open cur_acc
declare @acid int,@count int
fetch First from cur_acc into @acid
while @@fetch_status=0  
 begin
   select @count=COUNT(*) from Accounts where Ac_ID=@acid
   if @count<1
   begin
		select @num=@num+1
		DELETE FROM [LogForStudentStudy] where Ac_ID=@acid
		--print @count
   end  
   --Update TestPaper Set [Cou_Name]=@couname Where Cou_ID=@couid  
   fetch next from cur_acc into @acid  
 end   
--�رղ��ͷ��α�
close cur_acc
deallocate cur_acc

print @num

/*ɾ��һЩ�γ��Ѿ������ڣ���ѧϰ��¼��������ѧϰ��¼������*/

set @num=0
declare cur_course cursor scroll
for select cou_id  from LogForStudentStudy group by cou_id
open cur_course
declare @couid bigint
fetch First from cur_course into @couid
while @@fetch_status=0  
 begin
   select @count=COUNT(*) from Course where cou_id=@couid
   if @count<1
   begin
		select @num=@num+1
		DELETE FROM LogForStudentStudy where cou_id=@couid
		--print @couid
   end  
   fetch next from cur_course into @couid  
 end   
--�رղ��ͷ��α�
close cur_course
deallocate cur_course

print @num


/*ɾ��һЩ�½��Ѿ������ڣ���ѧϰ��¼��������ѧϰ��¼������*/

set @num=0
declare cur_outline cursor scroll
for select ol_id  from LogForStudentStudy group by ol_id
open cur_outline
declare @olid bigint
fetch First from cur_outline into @olid
while @@fetch_status=0  
 begin
   select @count=COUNT(*) from Outline where ol_id=@olid
   if @count<1
   begin
		select @num=@num+1
		DELETE FROM LogForStudentStudy where ol_id=@olid
		--print @olid
   end  
   fetch next from cur_outline into @olid  
 end   
--�رղ��ͷ��α�
close cur_outline
deallocate cur_outline

print @num