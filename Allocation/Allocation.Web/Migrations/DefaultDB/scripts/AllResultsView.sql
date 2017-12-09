create view allot.AllResultsView
as
select ApplicationUnit, MasterAwb, SUM(Amount) as Amount from [allot].[DeclarationData] 
GROUP by ApplicationUnit,MasterAwb