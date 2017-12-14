using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allocation.Modules.Common
{
    public class GetListResponse<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<T> Entities { get; set; }
    }
}