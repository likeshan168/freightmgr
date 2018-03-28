
namespace Allocation
{
    using Serenity.Services;
    using System;
    using System.Collections.Generic;

    public class MyExcelImportRequest : ServiceRequest
    {
        public int ApiId { get; set; }
        public String FileName { get; set; }
    }
}