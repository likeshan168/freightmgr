
namespace Allocation.Allot.Forms
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [FormScript("Allot.Api")]
    [BasedOnRow(typeof(Entities.ApiRow))]
    public class ApiForm
    {
        [HalfWidth]
        public String Title { get; set; }
        [HalfWidth]
        public String ApiUrl { get; set; }
        [HalfWidth]
        public String UserName { get; set; }
        [HalfWidth]
        public String Password { get; set; }
        [HalfWidth]
        public String AppKey { get; set; }
        [HalfWidth]
        public Decimal Price { get; set; }
        [HalfWidth]
        public Boolean IsEnabled { get; set; }
        [HalfWidth]
        public int ReuseCount { get; set; }
    }
}