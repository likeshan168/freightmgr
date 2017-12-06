using Serenity;
using System;
using System.IO;
using System.Web.Hosting;

namespace Allocation.Administration
{
    public class FakeSMSService : ISMSService
    {
        public void Send(string phoneNumber, string text, string reason)
        {
            Check.NotNullOrWhiteSpace(phoneNumber, "phoneNumber");

            var path = HostingEnvironment.MapPath("~/App_Data/SMS");
            Directory.CreateDirectory(path);

            var fileName = Path.Combine(path, phoneNumber + ".txt");
            File.AppendAllLines(fileName, new string[]
            {
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ": " + 
                text,
                ""
            });
        }
    }
}