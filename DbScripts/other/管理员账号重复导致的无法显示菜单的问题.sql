
/*�޸���1.0����2.0ʱ������Ա�˺��ظ����µ��޷���ʾ�˵�������*/


/*�����������˺�ȫ�����óɹ���Ա�������ܣ�*/
declare @root_orgid int,@posid int
select @root_orgid= Org_ID from Organization where Org_IsRoot=1
select @posid= Posi_Id from Position where Org_ID=@root_orgid and Posi_IsAdmin=1
update EmpAccount set Posi_Id=@posid  where  Org_ID=@root_orgid
/*�����ظ��Ĺ���Ա�˺ţ�����ظ��˺���+���*/
declare cursor_obj  cursor scroll
for select ROW_NUMBER() over(order by acc_id) as 'row',n.Acc_AccName,emp.Acc_Id,emp.Org_ID from
	 (select Acc_AccName,COUNT(*) as 'num' from EmpAccount  group by Acc_AccName) as n
	inner join
	(select * from EmpAccount) as emp
	on n.Acc_AccName=emp.Acc_AccName where num>1
open cursor_obj
declare @accname nvarchar(500),@accid int,@row int,@orgid int

fetch First from cursor_obj into @row,@accname,@accid,@orgid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin 
	--print @accname
    if @root_orgid!=@orgid
    begin
		update EmpAccount set Acc_AccName+=CAST(@row as nvarchar(50))  where Acc_Id=@accid
		print @accname
		print @accid
    end
   
   fetch next from cursor_obj into @row,@accname,@accid,@orgid
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj