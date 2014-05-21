using System;

namespace GameSocket
{
    /// <summary>
    /// CreateConnectionAsyncArgs 的摘要说明
    /// 
    /// <para>____________________________________________________________</para>
    /// <para>Version：V1.0.0</para>
    /// <para>Namespace：GameSocket</para>
    /// <para>Author: wboy    Time：2014/4/25 10:24:46</para>
    /// </summary>
    public class CreateConnectionAsyncArgs : EventArgs
    {
        public bool ConnectionOk
        {
            get;
            private set;
        }

        public CreateConnectionAsyncArgs(bool connnectionOk)
        {
            this.ConnectionOk = connnectionOk;
        }
    }
}
