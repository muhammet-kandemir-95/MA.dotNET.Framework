using System;
using System.IO;
using System.Reflection;

namespace MA.dotNet.Framework.Web.ClassLibrary.HTTPFileServer.ConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string localDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            HTTPServer server = new HTTPServer(localDirectory, 12345);
            server.Start();
        }
    }
}
