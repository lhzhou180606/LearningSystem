/*�����Ծ������Ŀγ�����*/
alter table TestPaper add [Cou_Name] [nvarchar](100) NULL
go
/*�Ծ��еĿγ�������γ̱�ͬ��*/
declare cur_couid cursor scroll
for select cou_id from TestPaper group by cou_id
open cur_couid
declare @couid int,@couname nvarchar(100)
fetch First from cur_couid into @couid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
   select @couname=cou_name from Course where Cou_ID=@couid
   Update TestPaper Set [Cou_Name]=@couname Where Cou_ID=@couid  --�޸ĵ�ǰ��
   fetch next from cur_couid into @couid   --�ƶ��α�
 end   
--�رղ��ͷ��α�
close cur_couid
deallocate cur_couid