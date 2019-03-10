using MA.dotNET.Framework.Standart.ClassLibrary.MimeReader;
using MA.dotNET.Framework.Standart.ClassLibrary.QueueThread;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace MA.dotNET.Framework.Standart.ClassLibrary.SMTPServer
{
    public class Server : IDisposable
    {
        #region Constructors
        public Server(int port, int coreCount = 4, int dataLimit = -1, int timeOut = -1)
        {
            this.Port = port;
            this.Listener = new TcpListener(IPAddress.Any, this.Port);

            this.DataLimit = dataLimit;
            this.TimeOut = timeOut;

            this.QueueServer = new QueueServer(coreCount);
        }

        public Server(X509Certificate sslCertificate, int port, int coreCount = 4, int dataLimit = -1, int timeOut = -1) : this(port, coreCount, dataLimit, timeOut)
        {
            this.SslCertificate = sslCertificate;
            this.UseSsl = true;
        }
        #endregion

        #region Variables
        /// <summary>
        /// SMTP Server port
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// This is for maximum data limit.
        /// <para></para>
        /// If user send bigger than max limit then server substring by max limit and not pull otherd datas
        /// </summary>
        public int DataLimit { get; private set; } = -1;
        /// <summary>
        /// As second
        /// </summary>
        public int TimeOut { get; private set; } = -1;
        public bool UseSsl { get; private set; } = false;
        public X509Certificate SslCertificate { get; private set; }
        /// <summary>
        /// Server
        /// </summary>
        public TcpListener Listener { get; private set; }
        internal QueueServer QueueServer = null;

        internal List<Client> ClientsAsList = new List<Client>();
        public Client[] Clients
        {
            get
            {
                lock (this.ClientsAsList)
                {
                    return this.ClientsAsList.ToArray();
                }
            }
        }

        private bool _disposed = false;
        #endregion

        #region Events
        public delegate bool Authentication(string username, string password);
        public event Authentication OnAuthentication;

        public delegate void GetEmail(string from, string to, string username, string password, EmailMime mime);
        public event GetEmail OnGetEmail;
        #endregion

        #region Methods
        internal bool RunAuthentication(string username, string password)
        {
            if (this.OnAuthentication != null)
                return this.OnAuthentication(username, password);
            return true;
        }

        internal void RunGetEmail(string from, string to, string username, string password, EmailMime mime)
        {
            if (this.OnGetEmail != null)
                this.OnGetEmail(from, to, username, password, mime);
        }

        public void Start()
        {
            try
            {
                this.Listener.Start();
                this.QueueServer.Start();

                while (_disposed == false)
                {
                    Client newClient = new Client(this);
                    newClient.Start();

                    lock (this.ClientsAsList)
                    {
                        this.ClientsAsList.Add(newClient);
                    }
                }
            }
            catch (Exception ex)
            {
                if (this._disposed == false)
                    throw ex;
            }
        }

        public void Dispose()
        {
            this._disposed = true;

            lock (ClientsAsList)
            {
                var clientsAsArray = ClientsAsList.ToArray();
                foreach (var client in clientsAsArray)
                    client.Dispose();
            }

            try
            { this.Listener.Stop(); }
            catch { }
        }
        #endregion
    }
}
