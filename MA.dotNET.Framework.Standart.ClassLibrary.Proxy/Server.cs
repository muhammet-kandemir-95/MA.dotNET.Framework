using MA.dotNET.Framework.Standart.ClassLibrary.QueueThread;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace MA.dotNET.Framework.Standart.ClassLibrary.Proxy
{
    public class Server : IDisposable
    {
        #region Constructors
        public Server(string hostname, int port, int proxyServerPort, int coreCount = 4)
        {
            this.ProxyServerPort = proxyServerPort;
            this.Listener = new TcpListener(IPAddress.Any, ProxyServerPort);

            this.Hostname = hostname;
            this.Port = port;

            this.QueueServer = new QueueServer(coreCount);
        }
        #endregion

        #region Variables
        public int ProxyServerPort { get; private set; }

        public string Hostname { get; private set; }
        public int Port { get; private set; }

        internal QueueServer QueueServer = null;
        /// <summary>
        /// Listener for read the original client
        /// </summary>
        public TcpListener Listener { get; private set; }

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

        #region Methods
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

            this.QueueServer.Dispose();

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
