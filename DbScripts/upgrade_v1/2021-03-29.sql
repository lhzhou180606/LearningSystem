--2021-3-29
/*���ӵ�¼У��֤���ֶ�*/
alter table EmpAccount add Acc_CheckUID nvarchar(255)
go
/*�ʽ���ˮ����ѧԱ��Ϣ*/
alter  table MoneyAccount add Ac_AccName  nvarchar(50)
go
alter  table MoneyAccount add Ac_Name  nvarchar(50)
go

declare my_cursor cursor scroll
for select Ac_ID, Ac_AccName,Ac_Name from Accounts
open my_cursor
declare @id int,@name nvarchar(100),@accname nvarchar(100)
fetch First from my_cursor into @id,@accname,@name 
while @@fetch_status=0  
 begin
   
   Update MoneyAccount Set Ac_AccName=@accname,Ac_Name=@name Where Ac_ID=@id  --�޸ĵ�ǰ��
   fetch next from my_cursor  into @id,@accname,@name 
 end   
--�رղ��ͷ��α�
close my_cursor
deallocate my_cursor
