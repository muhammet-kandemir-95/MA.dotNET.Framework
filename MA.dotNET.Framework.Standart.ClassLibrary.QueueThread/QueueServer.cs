using System;
using System.Collections.Generic;
using System.Threading;

namespace MA.dotNET.Framework.Standart.ClassLibrary.QueueThread
{
    public class QueueServer : IDisposable
    {
        #region Constructors
        public QueueServer(int coreCount)
        {
            this.CoreCount = coreCount;
            if (this.CoreCount > 1)
            {
                for (int i = 0; i < this.CoreCount; i++)
                {
                    this._threads.Add(new Thread(() =>
                    {
                        while (this._disposed == false)
                        {
                            Action firstAction = null;
                            lock (this._actions)
                                if (this._actions.Count > 0)
                                    firstAction = this._actions.Dequeue();

                            if (firstAction != null)
                                firstAction();

                            Thread.Sleep(1);
                        }
                    }));
                }
            }
        }
        #endregion

        #region Variables
        public int CoreCount { get; private set; }

        Queue<Action> _actions = new Queue<Action>();
        bool _disposed = false;

        List<Thread> _threads = new List<Thread>();
        #endregion

        #region Methods
        public void Start()
        {
            lock (this._threads)
            {
                foreach (var thread in this._threads)
                    thread.Start();
            }
        }

        public void AddAction(Action action)
        {
            if (this.CoreCount > 1)
            {
                lock (this._actions)
                    this._actions.Enqueue(action);
            }
            else
                action();
        }

        public void Dispose()
        {
            _disposed = true;
            lock (this._threads)
            {
                foreach (var thread in this._threads)
                    thread.Abort();
            }
            lock (this._actions)
            {
                _actions.Clear();
            }
        }
        #endregion
    }
}
