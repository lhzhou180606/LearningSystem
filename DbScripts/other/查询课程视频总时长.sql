
select cou.*, 
CASE WHEN s.count is null THEN 0 ELSE s.count END as 'ѧԱ��',
CASE WHEN q.count is null THEN 0 ELSE q.count END as '������'
from 
(
	/*��ѯ�γ���ʱ��,�½���*/
	select c.Cou_ID,c.Cou_Name as '�γ�', 
	CASE WHEN olcount is null THEN 0 ELSE olcount END as '�½���',
	CASE WHEN c.Th_Name='��' THEN ''  WHEN c.Th_Name is null THEN ''  ELSE c.Th_Name END as '��ʦ',
	CASE WHEN dur.duration is null THEN 0 ELSE dur.duration END  as '��ʱ��' 
	from Course as c left join
	(
		select cou_id,COUNT(*) as olcount, SUM(As_Duration) as 'duration' from (
			select Ol_ID,Ol_Name,Cou_ID,Ol_UID,acc.* from 
			(
				(select * from Outline) as ol
				left join 
				(SELECT [As_Id]
					  ,[As_Name]      
					  ,[As_Uid]    
					  ,[As_Duration]    
				  FROM [Accessory]
				where As_Type='CourseVideo') as acc on ol.Ol_UID=acc.As_Uid
			) 
		) as data group by cou_id
	) as dur on  c.Cou_ID=dur.Cou_ID
) as cou left join (select Cou_ID,COUNT(*) as 'count' from Student_Course group by Cou_ID) as s
on cou.Cou_ID=s.Cou_ID
left join (select Cou_ID,COUNT(*) as 'count' from Questions group by Cou_ID) as q
on cou.Cou_ID=q.Cou_ID