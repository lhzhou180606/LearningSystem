--2016-06-30
/*�����Գɼ���¼��Ϊnvarchar(max)����*/
alter table [ExamResults] ALTER COLUMN Exr_Results [nvarchar](max) NULL
go
/*���½����ݸ�Ϊnvarchar(max)����*/
alter table [Outline] ALTER COLUMN Ol_Intro[nvarchar](max) NULL
go

--�������ȡ����Ĵ洢����
if (exists (select * from sys.objects where name = 'PROC_QuesRandom'))
    drop proc PROC_QuesRandom
go
create proc PROC_QuesRandom(
    @orgid int, 
    @sbjid int,
    @couid int,
    @olid int,
    @type int,
    @diff1 int,
    @diff2 int,
    @isUse bit,
    @count int
)
as
declare @sql varchar(1000);
declare @where varchar(1000);
Declare @d Datetime;
set @d=getdate();
SET @sql = 'SELECT * FROM Questions';
--��ѯ����
set @where=' Qus_Diff>='+cast(@diff1 as varchar(1000))
    +' and Qus_Diff<='+ cast(@diff2 as varchar(1000))+' and Qus_IsError=0 and Qus_IsUse='
    +cast(@isUse as varchar(1000))+' and org_id=' + cast(@orgid as varchar(1000)) + ' ';
--�������ͣ����С�ڵ���0�������Ӹ�����
if (@type>0)
 begin
    set @where=@where+' and Qus_Type=' + cast(@type as varchar(1000)) ;
 end
--רҵ������
if (@sbjid>0)
 begin
    set @where=@where+' and Sbj_ID=' + cast(@sbjid as varchar(1000)) ;
 end
--�γ̵�����
if (@couid>0)
 begin
    set @where=@where+' and Cou_ID=' + cast(@couid as varchar(1000)) ;
 end
--�½ڵ�����
if (@olid>0)
 begin
    set @where=@where+' and Ol_ID=' + cast(@olid as varchar(1000)) ;
 end
 --����
 if (@count<1)
 begin
    set @count=99999;
 end 
--ƴ�����յ�sql���
set @sql='select top ' + cast(@count as varchar(1000)) + ' newid() as n,  *  from Questions where ' + @where + ' order by n';
set @sql = 'select * from (' + @sql + ') as t order by t.Qus_Type asc';
--set @sql='select top ' + @count + ' newid() as n,  *  from (select * from Questions where ' + @where + ') as tm order by n';
--print @where;
print @sql;
exec (@sql);

--select [���ִ�л���ʱ��(����)]=Datediff(ms,@d,Getdate());
print '[���ִ�л���ʱ��(����)]:'+cast(Datediff(ms,@d,Getdate()) as varchar(1000));
go

