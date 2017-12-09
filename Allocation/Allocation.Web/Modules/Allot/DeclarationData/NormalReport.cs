using System.DirectoryServices.AccountManagement;
using System.Linq;
using Allocation.Administration;

namespace Allocation.Allot
{
    using Entities;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using Serenity.Reporting;
    using System;
    using System.Collections.Generic;
    using Serenity;
    using Dapper;

    [Report("Allot.NormalReport")]
    [ReportDesign("~/Modules/Allot/DeclarationData/NormalReport.cshtml")]
    [RequiredPermission(AllotPermissionKeys.DeclarationData.View)]
    public class NormalReport : IReport, ICustomizeHtmlToPdf
    {
        public string SubAwb { get; set; }
        public List<string> MasterAwb { get; set; }

        public object GetData()
        {
            try
            {
                var data = new NormalReportData();

                using (var connection = SqlConnections.NewFor<DeclarationDataRow>())
                {
                    var flds = DeclarationDataRow.Fields;
                    var instance = Activator.CreateInstance<DeclarationDataRow>();
                    var query = new SqlQuery().From(instance).Select(flds.TenantId, flds.TenantName, flds.MasterAwb,
                        flds.SubAwb,
                        flds.Amount, flds.IsChecked, flds.Flight);
                    if (MasterAwb != null && MasterAwb.Count != 0)
                    {
                        string whereStr = string.Empty;
                        foreach (var masterAwb in MasterAwb)
                        {
                            whereStr = $"MasterAwb in ('{masterAwb}',";
                        }

                        whereStr = whereStr.TrimEnd(',') + ")";
                        query.Where(whereStr);
                    }
                    if (!string.IsNullOrWhiteSpace(SubAwb))
                    {
                        query.Where($"SubAwb='{SubAwb}'");
                    }

                    if (Authorization.HasPermission(PermissionKeys.Tenants))
                    {
                        //管理员
                    }
                    else
                    {
                        var user = (UserDefinition) Authorization.UserDefinition;
                        query.Where($"T0.TenantId={user.TenantId}");
                    }

                    var rows = connection.Query<DeclarationDataRow>(query);

                    var declarationDataRows = rows as IList<DeclarationDataRow> ?? rows.ToList();
                    var q = from p in declarationDataRows
                        orderby p.TenantName, p.MasterAwb, p.Flight
//                        where p.IsChecked == StateKind.Checked
                        group p by new {p.TenantName, p.MasterAwb, p.Flight}
                        into grouped
                        select new ReportData2
                        {
                            ApplicationUnit = grouped.Key.TenantName,
                            Flight = grouped.Key.Flight,
                            MasterAwb = grouped.Key.MasterAwb,
                            Amount = grouped.Sum(m => m.Amount),
//                            RealAmount = grouped.Sum(m => m.Amount)
                        };
                    var dataReportData = q as IList<ReportData2> ?? q.ToList();
                    foreach (var qr in dataReportData)
                    {
                        qr.RealAmount = declarationDataRows.Where(p =>
                                p.TenantName == qr.ApplicationUnit && p.Flight == qr.Flight &&
                                p.MasterAwb == qr.MasterAwb &&
                                (p.IsChecked == StateKind.Checked))
                            .Sum(p => p.Amount);
                    }

                    data.ReportData = dataReportData.Where(p=>p.Amount == p.RealAmount);
                }

                return data;
            }
            catch (Exception ex)
            {
                ex.Log();
                return new NormalReportData();
            }
        }

        public void Customize(IHtmlToPdfOptions options)
        {
            // you may customize HTML to PDF converter (WKHTML) parameters here, e.g. 
            // options.MarginsAll = "2cm";
        }
    }

    public class NormalReportData
    {
        public IEnumerable<ReportData2> ReportData { get; set; }
    }

    public class ReportData2
    {
        public string ApplicationUnit { get; set; }
        public string Flight { get; set; }
        public string MasterAwb { get; set; }
        public int? Amount { get; set; }
        public int? RealAmount { get; set; }
    }
}