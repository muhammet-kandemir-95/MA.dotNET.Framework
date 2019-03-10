using MA.dotNet.Framework.Web.ClassLibrary.HTTPFileServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MA.dotNet.Framework.Web.ClassLibrary.HTTPFileServerTests
{
    [TestClass]
    public class HTTPServerTests
    {
        [TestMethod]
        public void OnlyWork()
        {
            string localDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            HTTPServer server = new HTTPServer(localDirectory, 12345);
            bool error = false;
            bool started = false;
            new Thread(() =>
            {
                try
                {
                    started = true;
                    server.Start();
                }
                catch
                {
                    error = true;
                }
            }).Start();

            DateTime testStartDate = DateTime.Now;
            while (started == false)
            {
                var diffTestDate = DateTime.Now.AddTicks(testStartDate.Ticks * -1);
                if (diffTestDate.Second > 10)
                {
                    Assert.Fail(message: "Timeout!");
                }
            }
            Assert.AreEqual(error, false, "HTTP Server not working!");
            server.Dispose();
        }
    }
}
