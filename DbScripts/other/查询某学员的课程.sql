/* ѧԱ�Ŀγ̣�һ����Ϊ����Ŀγ̣�һ����������ѧԱ������Ŀγ�*/
select * from 
(
	select ROW_NUMBER() OVER(Order by  Stc_EndTime desc, Ssc_ID desc) AS 'rowid', * from 
	(
		select muster.*, sc.Stc_EndTime,ssc.Ssc_ID
		--,ROW_NUMBER() OVER(Order by  ssc.Ssc_ID desc) AS 'ssc'
		from 
		(
			/*���������Ѿ���ɲ�ѯ����Χ����ֻ��Ϊ������ͷ�ҳ*/
			select cou.* from Course as cou left join  Student_Course as sc
			on cou.Cou_ID = sc.Cou_ID
			where sc.Ac_ID=2 and sc.Stc_IsEnable=1 and sc.Stc_Type!=5
			and (sc.Stc_StartTime<getdate() and  sc.Stc_EndTime>getdate())
		
			union
			--except

			select cou.* from Course as cou right join  StudentSort_Course as ssc
			on cou.Cou_ID = ssc.Cou_ID
			where ssc.Sts_ID=15012714616000002

		) as muster 
		 left join 	
		 (
			select * from  Student_Course as sc
			where sc.Ac_ID=2 and sc.Stc_IsEnable=1 and sc.Stc_Type!=5
			and (sc.Stc_StartTime<getdate() and  sc.Stc_EndTime>getdate())
			and sc.Cou_ID not in (select Cou_ID from  StudentSort_Course where Sts_ID=2)			
		 ) as sc on muster.Cou_ID = sc.Cou_ID
		 left join  
		 (
			select * from  StudentSort_Course where Sts_ID=2
		 ) as ssc  on muster.Cou_ID = ssc.Cou_ID
		
	)as t where Cou_Name like '%��%' 
	or 1=1
) as p where rowid>0 and rowid<=6