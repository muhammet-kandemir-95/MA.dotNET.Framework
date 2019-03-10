using MA.dotNET.Framework.Standart.ClassLibrary.MimeReader;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;

namespace MA.dotNET.Framework.Standart.ClassLibrary.SMTPServer
{
    public class Client
    {
        #region Constructors
        internal Client(Server server)
        {
            this.Server = server;
        }
        #endregion

        #region Variables
        public Server Server { get; private set; }

        public TcpClient Tcp { get; private set; }
        bool _startedSsl = false;
        public Stream Stream
        {
            get
            {
                if (this.Server.UseSsl == true && _startedSsl == true)
                    return this.SslStream;
                return NetworkStream;
            }
        }
        public NetworkStream NetworkStream { get; private set; }
        public SslStream SslStream { get; private set; }

        private bool _disposed = false;

        private bool _CheckConnectionAndDisposedForOriginalClient
        {
            get
            {
                if (this._disposed == true)
                    return false;

                lock (this.Tcp)
                {
                    if (this.Tcp.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (this.Tcp.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        internal string DataBeforeMessage = "";
        private DateTime _startDateTime = DateTime.Now;
        internal bool TimeoutCheck
        {
            get
            {
                return this.Server.TimeOut == -1 || (DateTime.Now - this._startDateTime).TotalSeconds <= this.Server.TimeOut;
            }
        }
        #endregion
        
        #region Methods
        internal string ReadLine()
        {
            StringBuilder msg = new StringBuilder();
            if (string.IsNullOrEmpty(DataBeforeMessage) == false)
            {
                var splitByEnterForBeforeDataMessage = DataBeforeMessage.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                if (splitByEnterForBeforeDataMessage.Length > 1)
                {
                    var msgResult = splitByEnterForBeforeDataMessage[0];
                    splitByEnterForBeforeDataMessage[0] = "";
                    this.DataBeforeMessage = string.Join(Environment.NewLine, splitByEnterForBeforeDataMessage).Substring(2);

                    return msgResult;
                }
                else
                    this.DataBeforeMessage = "";
            }

            var msgAsString = "";
            while (this._CheckConnectionAndDisposedForOriginalClient == true && this.TimeoutCheck)
            {
                if (this.NetworkStream.DataAvailable == true)
                {
                    lock (this.Stream)
                    {
                        byte[] buffer = new byte[1024]; // read in chunks of 1KB
                        int bytesRead;
                        while (this._CheckConnectionAndDisposedForOriginalClient && this.NetworkStream.DataAvailable && (bytesRead = this.Stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            var dataAsString = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                            msg.Append(dataAsString);

                            if (this.Server.DataLimit != -1 && this.Server.DataLimit < msg.Length)
                                throw new OverflowException();

                            msgAsString = msg.ToString();
                            if (msgAsString.Contains(Environment.NewLine))
                                break;
                        }
                    }
                    if (msgAsString.Contains(Environment.NewLine))
                        break;
                }
                Thread.Sleep(1);
            }
            msg.Clear();

            var splitByEnter = msgAsString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            if (splitByEnter.Length > 1)
            {
                msgAsString = splitByEnter[0];
                splitByEnter[0] = "";
                this.DataBeforeMessage = string.Join(Environment.NewLine, splitByEnter).Substring(2);
            }

            return msgAsString;
        }

        internal void WriteLine(string msg)
        {
            lock (this.Stream)
            {
                var msgAsBuffer = Encoding.ASCII.GetBytes(msg + Environment.NewLine);
                this.Stream.Write(msgAsBuffer, 0, msgAsBuffer.Length);
                this.Stream.Flush();
            }
        }

        internal void Start()
        {
            this.Tcp = this.Server.Listener.AcceptTcpClient();
            this.Tcp.ReceiveTimeout = this.Server.TimeOut == -1 ? -1 : this.Server.TimeOut * 1000;
            this.Tcp.SendTimeout = this.Tcp.ReceiveTimeout;

            this.NetworkStream = this.Tcp.GetStream();
            if (this.Server.UseSsl == true)
                this.SslStream = new SslStream(this.NetworkStream);

            #region ListenerThread
            this.Server.QueueServer.AddAction(() =>
            {
                try
                {
                    this._startDateTime = DateTime.Now;
                    WriteLine("220 MA.dotNET.Framework.Standart.ClassLibrary.SMTPServer -- MA");

                    string from = "";
                    string to = "";
                    string username = "";
                    string password = "";
                    bool authCompleted = false;
                    while (this.TimeoutCheck)
                    {
                        var msg = this.ReadLine().ToString();
                        if (msg.StartsWith("QUIT", false, CultureInfo.InvariantCulture))
                            throw new Exception();
                        else if (msg.StartsWith("EHLO", false, CultureInfo.InvariantCulture))
                        {
                            if (this.Server.UseSsl == true)
                            {
                                WriteLine("250-MA.dotNET.Framework.Standart.ClassLibrary.SMTPServer");
                                WriteLine("250-STARTTLS");
                                WriteLine("250-AUTH PLAIN LOGIN");
                                WriteLine("250 SMTPUTF8");
                            }
                            else
                            {
                                WriteLine("250-MA.dotNET.Framework.Standart.ClassLibrary.SMTPServer");
                                WriteLine("250-AUTH PLAIN LOGIN");
                                WriteLine("250 SMTPUTF8");
                            }
                        }
                        else if (msg.StartsWith("STARTTLS", false, CultureInfo.InvariantCulture))
                        {
                            WriteLine("220 OK");
                            this.SslStream.AuthenticateAsServer(this.Server.SslCertificate, false, SslProtocols.Tls, true);
                            this._startedSsl = true;
                        }
                        else if (msg.StartsWith("AUTH", false, CultureInfo.InvariantCulture))
                        {
                            var splitMsgBySpace = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (splitMsgBySpace.Length > 2)
                            {
                                username = Encoding.UTF8.GetString(Convert.FromBase64String(splitMsgBySpace[2]));
                                WriteLine("334 UGFzc3dvcmQ6");
                                password = Encoding.UTF8.GetString(Convert.FromBase64String(ReadLine()));
                            }
                            else
                            {
                                WriteLine("334 VXNlcm5hbWU6");
                                username = Encoding.UTF8.GetString(Convert.FromBase64String(ReadLine()));
                                WriteLine("334 UGFzc3dvcmQ6");
                                password = Encoding.UTF8.GetString(Convert.FromBase64String(ReadLine()));
                            }

                            authCompleted = true;
                            if (this.Server.RunAuthentication(username, password) == false)
                                throw new ArgumentException();

                            WriteLine("235 2.7.0 Authentication successful");
                        }
                        else if (msg.StartsWith("RCPT TO", false, CultureInfo.InvariantCulture))
                        {
                            to = msg.Split('<', '>')[1];
                            WriteLine("250 OK");
                        }
                        else if (msg.StartsWith("MAIL FROM", false, CultureInfo.InvariantCulture))
                        {
                            from = msg.Split('<', '>')[1];
                            WriteLine("250 OK");
                        }
                        else if (msg.StartsWith("DATA", false, CultureInfo.InvariantCulture))
                        {
                            if (authCompleted == false)
                                if (this.Server.RunAuthentication(username, password) == false)
                                    throw new ArgumentException();

                            WriteLine("354 Start mail input; end with");
                            StringBuilder mailContent = new StringBuilder();
                            while (this.TimeoutCheck)
                            {
                                var line = this.ReadLine();
                                if (line.Length == 1 && line.ToString() == ".")
                                {
                                    mailContent.Append(line);
                                    break;
                                }
                                mailContent.Append(line + Environment.NewLine);
                            }

                            string mailContentAsString = mailContent.ToString();
                            mailContent.Clear();

                            this.Server.RunGetEmail(from, to, username, password, new EmailMime(mailContentAsString));
                            mailContentAsString = "";

                            WriteLine("250 OK");
                        }
                    }
                }
                catch (Exception ex)
                { }

                this.Dispose();
            });
            #endregion
        }

        internal void Dispose()
        {
            this._disposed = true;
            lock (this.Server.ClientsAsList)
            {
                if (this.Server.ClientsAsList.Count > 0)
                    this.Server.ClientsAsList.Remove(this);
            }
            
            try { this.Tcp.Close(); }
            catch { }
        }
        #endregion
    }
}
