
namespace Allocation.Common.Services
{
    using Serenity.Data;
    using System;
    using System.Linq;

    public sealed class SqlApplicationLock
    {
        public static int Get(IUnitOfWork uow, string resourceName,
            string lockMode = "Exclusive", int lockTimeout = 0)
        {
            return uow.Connection.Query<int?>(
                String.Format(
                    "DECLARE @result int;\n" +
                    "EXEC @result = sp_getapplock @Resource = {0}, " + 
                        "@LockMode = {1}, @LockTimeout = {2};\n" +
                    "SELECT @result;",
                    resourceName.ToSql(), 
                    lockMode.ToSql(), 
                    lockTimeout)
                ).First() ?? Int32.MinValue;
        }
    }
}