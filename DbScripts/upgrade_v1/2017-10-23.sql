--2017-10-23

--2017-10-23

/*��ʦ���۱��еĻ���id������Ϊ��ʦ���ڵĻ���id������֮ǰ��bug����û����һ�*/
update [TeacherComment] set Org_ID=1
go
alter table [TeacherComment] ALTER COLUMN Org_ID int NOT NULL
go
declare cur_thid cursor scroll
for select th_id from TeacherComment group by th_id
open cur_thid
declare @orgid int,@thid int
set @orgid=0
fetch First from cur_thid into @thid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
   select @orgid=Org_ID from Teacher where th_ID=@thid
   Update TeacherComment Set [Org_ID]=@orgid Where th_ID=@thid  --�޸ĵ�ǰ��
   --print @orgid
   fetch next from cur_thid into @thid   --�ƶ��α�
 end   
--�رղ��ͷ��α�
close cur_thid
deallocate cur_thid