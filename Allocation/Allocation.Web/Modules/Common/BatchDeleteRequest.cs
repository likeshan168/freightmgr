using System.Collections.Generic;
using Serenity.Services;

namespace Allocation.Modules.Common
{
    public class BatchDeleteRequest : ServiceRequest
    {
        public List<object> EntityIds;
    }
}