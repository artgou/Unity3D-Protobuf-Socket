using System;

namespace GameSocket
{
    /// <summary>
    /// 套接字触发
    /// 
    /// <para>____________________________________________________________</para>
    /// <para>Version：V1.0.0</para>
    /// <para>Namespace：GameSocket</para>
    /// <para>Author: wboy    Time：2014/4/25 11:39:30</para>
    /// </summary>
    public abstract class IProcessCMD
    {
        public abstract void IncomingData(byte[] data, int actualSize);
    }
}
