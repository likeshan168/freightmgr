create view allot.NormalResultsView
as
select a.ApplicationUnit,a.Flight, a.MasterAwb, a.Amount, normal.Amount as NoCheckedAmount  from 
(select ApplicationUnit,Flight, MasterAwb, SUM(Amount) as Amount from [allot].[DeclarationData] 
GROUP by ApplicationUnit,Flight,MasterAwb) as a
 join 
(select ApplicationUnit, Flight, MasterAwb, SUM(Amount) as Amount from [allot].[DeclarationData] 
WHERE IsChecked =2
GROUP by ApplicationUnit,Flight,MasterAwb) normal
on 
a.ApplicationUnit = normal.ApplicationUnit and a.MasterAwb = normal.MasterAwb and a.Flight = normal.Flight and a.Amount = normal.Amount