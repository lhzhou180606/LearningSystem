/*ɾ��һЩ�γ��Ѿ������ڣ���ѧϰ��¼��������ѧϰ��¼������*/
declare @num int
set @num=0
declare cur_acc cursor scroll
for select Ac_ID  from Student_Ques group by Ac_ID
open cur_acc
declare @acid int,@count int
select @num=0
fetch First from cur_acc into @acid
while @@fetch_status=0  
 begin
   select @count=COUNT(*) from Accounts where Ac_ID=@acid
   if @count<1
   begin
		select @num=@num+1
		DELETE FROM Student_Ques where Ac_ID=@acid
		print @count
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
for select cou_id  from Student_Ques group by cou_id
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
