create view allot.AbnormalResultsView
as
select t.ApplicationUnit,t.Flight,t.MasterAwb,t.Amount,t.NoCheckedAmount,isnull(oc.Amount, 0) OverAmount from 
(
select a.ApplicationUnit,a.Flight, a.MasterAwb, a.Amount, nc.Amount as NoCheckedAmount from 
(select ApplicationUnit, Flight, MasterAwb, SUM(Amount) as Amount from [allot].[DeclarationData] 
GROUP by ApplicationUnit, Flight ,MasterAwb) as a
 join 
(select ApplicationUnit,Flight, MasterAwb, SUM(Amount) as Amount from [allot].[DeclarationData] 
WHERE IsChecked =1
GROUP by ApplicationUnit,Flight,MasterAwb) nc
on 
a.ApplicationUnit = nc.ApplicationUnit and a.Flight = nc.Flight and a.MasterAwb = nc.MasterAwb
) as t
left join 
(select ApplicationUnit,Flight, MasterAwb, SUM(Amount) as Amount from [allot].[DeclarationData] 
WHERE IsChecked =3
GROUP by ApplicationUnit,Flight,MasterAwb) oc
on t.ApplicationUnit = oc.ApplicationUnit and t.Flight = oc.Flight and t.MasterAwb= oc.MasterAwb