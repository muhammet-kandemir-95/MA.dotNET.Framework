using MA.dotNET.Framework.Standart.ClassLibrary.QueueThread;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MA.dotNet.Framework.Web.ClassLibrary.HTTPFileServer
{
    public class HTTPServer : IDisposable
    {
        #region Constructors
        public HTTPServer(string rootDirectory, int port, int coreCount = 1)
        {
            this.RootDirectory = rootDirectory;
            this.Port = port;

            this.Listener = new HttpListener();
            this.Listener.Prefixes.Add("http://*:" + this.Port.ToString() + "/");

            this.QueueServer = new QueueServer(coreCount);
        }
        #endregion

        #region Variables
        public string RootDirectory { get; private set; }
        public int Port { get; private set; }
        public HttpListener Listener { get; private set; }
        private bool _disposed = false;
        internal QueueServer QueueServer = null;

        private static Dictionary<string, string> _mimeTypes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            #region extension to MIME type list
            {".html", "text/html"},
            {".js", "application/x-javascript"},
            {".css", "text/css"},
            {".img", "application/octet-stream"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".png", "image/png"},
            {".txt", "text/plain"},

            {".asf", "video/x-ms-asf"},
            {".asx", "video/x-ms-asf"},
            {".avi", "video/x-msvideo"},
            {".bin", "application/octet-stream"},
            {".cco", "application/x-cocoa"},
            {".crt", "application/x-x509-ca-cert"},
            {".deb", "application/octet-stream"},
            {".der", "application/x-x509-ca-cert"},
            {".dll", "application/octet-stream"},
            {".dmg", "application/octet-stream"},
            {".ear", "application/java-archive"},
            {".eot", "application/octet-stream"},
            {".exe", "application/octet-stream"},
            {".flv", "video/x-flv"},
            {".gif", "image/gif"},
            {".hqx", "application/mac-binhex40"},
            {".htc", "text/x-component"},
            {".htm", "text/html"},
            {".ico", "image/x-icon"},
            {".iso", "application/octet-stream"},
            {".jar", "application/java-archive"},
            {".jardiff", "application/x-java-archive-diff"},
            {".jng", "image/x-jng"},
            {".jnlp", "application/x-java-jnlp-file"},
            {".mml", "text/mathml"},
            {".mng", "video/x-mng"},
            {".mov", "video/quicktime"},
            {".mp3", "audio/mpeg"},
            {".mpeg", "video/mpeg"},
            {".mpg", "video/mpeg"},
            {".msi", "application/octet-stream"},
            {".msm", "application/octet-stream"},
            {".msp", "application/octet-stream"},
            {".pdb", "application/x-pilot"},
            {".pdf", "application/pdf"},
            {".pem", "application/x-x509-ca-cert"},
            {".pl", "application/x-perl"},
            {".pm", "application/x-perl"},
            {".prc", "application/x-pilot"},
            {".ra", "audio/x-realaudio"},
            {".rar", "application/x-rar-compressed"},
            {".rpm", "application/x-redhat-package-manager"},
            {".rss", "text/xml"},
            {".run", "application/x-makeself"},
            {".sea", "application/x-sea"},
            {".shtml", "text/html"},
            {".sit", "application/x-stuffit"},
            {".swf", "application/x-shockwave-flash"},
            {".tcl", "application/x-tcl"},
            {".tk", "application/x-tcl"},
            {".war", "application/java-archive"},
            {".wbmp", "image/vnd.wap.wbmp"},
            {".wmv", "video/x-ms-wmv"},
            {".xml", "text/xml"},
            {".xpi", "application/x-xpinstall"},
            {".zip", "application/zip"},
            #endregion
        };
        #endregion

        #region Methods
        public void Start()
        {
            try
            {
                this.Listener.Start();
                this.QueueServer.Start();
                while (this._disposed == false)
                {
                    try
                    {
                        HttpListenerContext context = this.Listener.GetContext();
                        this.QueueServer.AddAction(() =>
                        {
                            try
                            {
                                // If file cached 
                                if (string.IsNullOrEmpty(context.Request.Headers.Get("Cache-Control")) == false && string.IsNullOrEmpty(context.Request.Headers.Get("Pragma")) == true)
                                    context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                                else
                                {
                                    string filename = context.Request.Url.AbsolutePath.Substring(1);
                                    if (string.IsNullOrEmpty(filename))
                                        filename = "index.html";
                                    filename = Path.Combine(this.RootDirectory, filename);
                                    filename = Path.GetFullPath(filename);

                                    if (filename.StartsWith(this.RootDirectory) == false)
                                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                    else
                                    {
                                        try
                                        {
                                            Stream input = new FileStream(filename, FileMode.Open);

                                            string mime;
                                            context.Response.ContentType = _mimeTypes.TryGetValue(Path.GetExtension(filename), out mime) ? mime : "application/octet-stream";
                                            context.Response.AddHeader("Cache-Control", "public,max-age=" + int.MaxValue);
                                            context.Response.AddHeader("Content-Encoding", "gzip");

                                            #region Before
                                            GZipStream gzip = new GZipStream(context.Response.OutputStream, CompressionMode.Compress, true);

                                            byte[] readBuffer = new byte[1024 * 16];
                                            int nbytes;
                                            while ((nbytes = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                                                gzip.Write(readBuffer, 0, nbytes);
                                            input.Close();
                                            gzip.Flush();

                                            context.Response.ContentLength64 = context.Response.OutputStream.Length;
                                            #endregion

                                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                                        }
                                        catch (Exception ex)
                                        {
                                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                        }
                                    }
                                }

                                context.Response.OutputStream.Close();
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                    }
                    catch (Exception ex)
                    {
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

            try { this.Listener.Stop(); } catch { }
        }
        #endregion
    }
}
