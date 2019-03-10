using MA.dotNET.Framework.Standart.ClassLibrary.MimeReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MA.dotNET.Framework.Standart.ClassLibrary.MimeReaderTests
{
    [TestClass]
    public class EmailMimeTests
    {
        [TestMethod]
        public void Example1()
        {
            string mimeData = @"MIME-Version: 1.0
From: you@yourcompany.com
To: user@hotmail.com
Date: 11 Aug 2018 15:45:42 +0300
Subject: this is a test email.
Content-Type: text/html; charset=us-ascii
Content-Transfer-Encoding: quoted-printable

=0D=0Athis is=3D0D=3D0D my test email body=0D=0Ab=0D=0A

.";

            var mime = new EmailMime(mimeData);
            Assert.AreEqual(mime.Parameters["MIME-Version"], "1.0");
            Assert.AreEqual(mime.Parameters["From"], "you@yourcompany.com");
            Assert.AreEqual(mime.Parameters["To"], "user@hotmail.com");
            Assert.AreEqual(mime.Parameters["Date"], "11 Aug 2018 15:45:42 +0300");
            Assert.AreEqual(mime.Subject, "this is a test email.");
            Assert.AreEqual(mime.Parameters["Content-Type"], "text/html; charset=us-ascii");
            Assert.AreEqual(mime.Parameters["Content-Transfer-Encoding"], "quoted-printable");

            Assert.AreEqual(mime.Body, Environment.NewLine + "this is=0D=0D my test email body" + Environment.NewLine + "b" + Environment.NewLine);
        }

        [TestMethod]
        public void Example2()
        {
            string mimeData = @"MIME-Version: 1.0
From: you@yourcompany.com
To: user@hotmail.com
Date: 11 Aug 2018 15:57:16 +0300
Subject: this is a test email.
Content-Type: text/html; charset=utf-8
Content-Transfer-Encoding: base64

DQp0aGlzIGlzPTBEPTBEIG15IHRlc3QgZW1haWwgYm9keQ0KYg0K

.";

            var mime = new EmailMime(mimeData);
            Assert.AreEqual(mime.Parameters["MIME-Version"], "1.0");
            Assert.AreEqual(mime.Parameters["From"], "you@yourcompany.com");
            Assert.AreEqual(mime.Parameters["To"], "user@hotmail.com");
            Assert.AreEqual(mime.Parameters["Date"], "11 Aug 2018 15:57:16 +0300");
            Assert.AreEqual(mime.Subject, "this is a test email.");
            Assert.AreEqual(mime.Parameters["Content-Type"], "text/html; charset=utf-8");
            Assert.AreEqual(mime.Parameters["Content-Transfer-Encoding"], "base64");

            Assert.AreEqual(mime.Body, Environment.NewLine + "this is=0D=0D my test email body" + Environment.NewLine + "b" + Environment.NewLine);
        }

        [TestMethod]
        public void Example3()
        {
            string mimeData = @"MIME-Version: 1.0
From: you@yourcompany.com
To: user@hotmail.com
Date: 11 Aug 2018 16:02:12 +0300
Subject: this is a test email.
Content-Type: text/html; charset=us-ascii
Content-Transfer-Encoding: quoted-printable

=0D=0A------this is=3D0D=3D0D my test email body=0D=0A=0D=0A=0D=0A=
-----b=0D=0A-----ff----------------

.";

            var mime = new EmailMime(mimeData);
            Assert.AreEqual(mime.Parameters["MIME-Version"], "1.0");
            Assert.AreEqual(mime.Parameters["From"], "you@yourcompany.com");
            Assert.AreEqual(mime.Parameters["To"], "user@hotmail.com");
            Assert.AreEqual(mime.Parameters["Date"], "11 Aug 2018 16:02:12 +0300");
            Assert.AreEqual(mime.Subject, "this is a test email.");
            Assert.AreEqual(mime.Parameters["Content-Type"], "text/html; charset=us-ascii");
            Assert.AreEqual(mime.Parameters["Content-Transfer-Encoding"], "quoted-printable");

            Assert.AreEqual(mime.Body, Environment.NewLine + "------this is=0D=0D my test email body" + Environment.NewLine + Environment.NewLine + Environment.NewLine + "-----b" + Environment.NewLine + "-----ff----------------");
        }

        [TestMethod]
        public void Example4()
        {
            string mimeData = @"MIME-Version: 1.0
From: you@yourcompany.com
To: user@hotmail.com
Date: 11 Aug 2018 16:10:03 +0300
Subject: this is a test email.
Content-Type: text/html; charset=utf-8
Content-Transfer-Encoding: base64

DQotLS0tLS10aGlzIGlzPTBEPTBEIG15IHRlc3QgZW1haWwgYm9keQ0KDQoNCi0tLS0t
Yg0KLS0tLS1mZi0tLS0tLS0tLS0tLS0tLS0=

.";

            var mime = new EmailMime(mimeData);
            Assert.AreEqual(mime.Parameters["MIME-Version"], "1.0");
            Assert.AreEqual(mime.Parameters["From"], "you@yourcompany.com");
            Assert.AreEqual(mime.Parameters["To"], "user@hotmail.com");
            Assert.AreEqual(mime.Parameters["Date"], "11 Aug 2018 16:10:03 +0300");
            Assert.AreEqual(mime.Subject, "this is a test email.");
            Assert.AreEqual(mime.Parameters["Content-Type"], "text/html; charset=utf-8");
            Assert.AreEqual(mime.Parameters["Content-Transfer-Encoding"], "base64");

            Assert.AreEqual(mime.Body, Environment.NewLine + "------this is=0D=0D my test email body" + Environment.NewLine + Environment.NewLine + Environment.NewLine + "-----b" + Environment.NewLine + "-----ff----------------");
        }

        [TestMethod]
        public void Example5()
        {
            string mimeData = @"MIME-Version: 1.0
From: you@yourcompany.com
To: user@hotmail.com
Date: 11 Aug 2018 16:10:03 +0300
Subject: =?utf-8?B?dGhpcyBpcyBhIHRlc3QgZW1haWwu?=
Content-Type: text/html; charset=utf-8
Content-Transfer-Encoding: base64

DQotLS0tLS10aGlzIGlzPTBEPTBEIG15IHRlc3QgZW1haWwgYm9keQ0KDQoNCi0tLS0t
Yg0KLS0tLS1mZi0tLS0tLS0tLS0tLS0tLS0=

.";

            var mime = new EmailMime(mimeData);
            Assert.AreEqual(mime.Parameters["MIME-Version"], "1.0");
            Assert.AreEqual(mime.Parameters["From"], "you@yourcompany.com");
            Assert.AreEqual(mime.Parameters["To"], "user@hotmail.com");
            Assert.AreEqual(mime.Parameters["Date"], "11 Aug 2018 16:10:03 +0300");
            Assert.AreEqual(mime.Subject, "this is a test email.");
            Assert.AreEqual(mime.Parameters["Content-Type"], "text/html; charset=utf-8");
            Assert.AreEqual(mime.Parameters["Content-Transfer-Encoding"], "base64");

            Assert.AreEqual(mime.Body, Environment.NewLine + "------this is=0D=0D my test email body" + Environment.NewLine + Environment.NewLine + Environment.NewLine + "-----b" + Environment.NewLine + "-----ff----------------");
        }
    }
}
