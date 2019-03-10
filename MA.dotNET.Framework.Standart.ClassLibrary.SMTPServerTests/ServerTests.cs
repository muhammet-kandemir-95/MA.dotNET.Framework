using MA.dotNET.Framework.Standart.ClassLibrary.MimeReader;
using MA.dotNET.Framework.Standart.ClassLibrary.SMTPServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace MA.dotNET.Framework.Standart.ClassLibrary.SMTPServerTests
{
    [TestClass]
    public class ServerTests
    {
        #region Constructors
        static ServerTests()
        {
            LocalDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // For unit tests working
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        }
        #endregion

        #region Variables
        public static string LocalDirectory = null;
        #endregion

        [TestMethod]
        public void Simple()
        {
            Server smtpServer = new Server(25);
            var testThread = new Thread(() =>
            {
                smtpServer.Start();
            });
            testThread.Start();

            MailMessage mail = new MailMessage("from@macompany.com", "to@macompany.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "localhost";
            mail.Subject = "This is Mail Subject";
            mail.Body = "Test email body";
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            client.Send(mail);

            smtpServer.Dispose();
        }

        [TestMethod]
        public void SimpleWithSSL()
        {
            return;
            Server smtpServer = new Server(new System.Security.Cryptography.X509Certificates.X509Certificate(Path.Combine(LocalDirectory, "test.pfx"), "1q2w3e4r"), 25);
            var testThread = new Thread(() =>
            {
                smtpServer.Start();
            });
            testThread.Start();

            MailMessage mail = new MailMessage("from@macompany.com", "to@macompany.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "localhost";
            mail.Subject = "This is Mail Subject";
            mail.Body = "Test email body";
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            client.EnableSsl = true;
            client.Send(mail);

            smtpServer.Dispose();
        }

        [TestMethod]
        public void FromToCheck()
        {
            string fromData = "";
            string toData = "";
            Server smtpServer = new Server(25);
            var testThread = new Thread(() =>
            {
                smtpServer.OnGetEmail += (string from, string to, string username, string password, EmailMime mime) =>
                {
                    fromData = from;
                    toData = to;
                };
                smtpServer.Start();

            });
            testThread.Start();

            MailMessage mail = new MailMessage("from@macompany.com", "to@macompany.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "localhost";
            mail.Subject = "This is Mail Subject";
            mail.Body = "Test email body";
            client.Send(mail);

            Assert.AreEqual(fromData, mail.From.Address);
            Assert.AreEqual(toData, mail.To[0].Address);
            smtpServer.Dispose();
        }

        [TestMethod]
        public void FromToPasswordCheck()
        {
            string usernameData = "";
            string passwordData = "";

            Server smtpServer = new Server(25);
            var testThread = new Thread(() =>
            {
                smtpServer.OnAuthentication += (string username, string password) =>
                {
                    usernameData = username;
                    passwordData = password;

                    return true;
                };
                smtpServer.Start();

            });
            testThread.Start();

            MailMessage mail = new MailMessage("from@macompany.com", "to@macompany.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("yourusername", "yourpassword");
            client.Host = "localhost";
            mail.Subject = "This is Mail Subject";
            mail.Body = "Test email body";
            client.Send(mail);

            Assert.AreEqual(usernameData, "yourusername");
            Assert.AreEqual(passwordData, "yourpassword");
            smtpServer.Dispose();
        }

        [TestMethod]
        public void AuthenticationWorkingCheck()
        {
            bool emailGetted = false;
            Server smtpServer = new Server(25);
            var testThread = new Thread(() =>
            {
                smtpServer.OnAuthentication += (string username, string password) =>
                {
                    return false;
                };
                smtpServer.OnGetEmail += (string from, string to, string username, string password, EmailMime mime) =>
                {
                    emailGetted = true;
                };
                smtpServer.Start();

            });
            testThread.Start();

            // For not change error message's
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            bool error = true;
            try
            {
                MailMessage mail = new MailMessage("from@macompany.com", "to@macompany.com");
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("yourusername", "yourpassword");
                client.Host = "localhost";
                mail.Subject = "This is Mail Subject";
                mail.Body = "Test email body";
                client.Send(mail);
                error = false;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Failure sending mail.");
            }
            Assert.AreEqual(emailGetted, false, "User authentication not working!");

            smtpServer.Dispose();

            if (error == false)
                Assert.Fail("Not throw user authentication error!");
        }

        [TestMethod]
        public void MimeCheck()
        {
            bool emailGetted = false;
            EmailMime mimeData = null;
            Server smtpServer = new Server(25);
            var testThread = new Thread(() =>
            {
                smtpServer.OnGetEmail += (string from, string to, string username, string password, EmailMime mime) =>
                {
                    emailGetted = true;
                    mimeData = mime;
                };
                smtpServer.Start();
            });
            testThread.Start();

            // For not change error message's
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            MailMessage mail = new MailMessage("from@macompany.com", "to@macompany.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "localhost";
            mail.Subject = "This is Mail Subject";
            mail.Body = "Test email body";
            client.Send(mail);

            Assert.AreEqual(emailGetted, true, "Mail read not working!");
            Assert.AreEqual(mimeData.Subject, mail.Subject);
            Assert.AreEqual(mimeData.Parameters["From"], mail.From.Address);
            Assert.AreEqual(mimeData.Parameters["To"], mail.To[0].Address);
            Assert.AreEqual(mimeData.Body, mail.Body);

            smtpServer.Dispose();
        }

        [TestMethod]
        public void MimeCheckWithUTF8()
        {
            bool emailGetted = false;
            EmailMime mimeData = null;
            Server smtpServer = new Server(25);
            var testThread = new Thread(() =>
            {
                smtpServer.OnGetEmail += (string from, string to, string username, string password, EmailMime mime) =>
                {
                    emailGetted = true;
                    mimeData = mime;
                };
                smtpServer.Start();
            });
            testThread.Start();

            // For not change error message's
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            MailMessage mail = new MailMessage("from@macompany.com", "to@macompany.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "localhost";
            mail.Subject = "This is Mail Subject‹‹‹";
            mail.Body = "Test email body";
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            client.Send(mail);

            Assert.AreEqual(emailGetted, true, "Mail read not working!");
            Assert.AreEqual(mimeData.Subject, mail.Subject);
            Assert.AreEqual(mimeData.Parameters["From"], mail.From.Address);
            Assert.AreEqual(mimeData.Parameters["To"], mail.To[0].Address);
            Assert.AreEqual(mimeData.Body, mail.Body);

            smtpServer.Dispose();
        }
    }
}
