select * from StudentSort_Course


/*ͳ�ƿγ��ж����� ��Ƶѧϰ��¼*/
select * from 
(
	select c.Cou_ID, cou_name, case when l.count is null then 0 else l.count end as 'count' from Course as c left join 
	(select Cou_ID, COUNT(*) as 'count' from LogForStudentStudy group by Cou_ID) as l
	on c.cou_id=l.cou_id
) as r order by count desc

