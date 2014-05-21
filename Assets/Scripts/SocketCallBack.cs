using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// SocketCallBack 的摘要说明
/// 
/// <para>____________________________________________________________</para>
/// <para>Version：V1.0.0</para>
/// <para>Namespace：GameSocket</para>
/// <para>Author: wboy    Time：2014/4/25 13:34:28</para>
/// </summary>
public class SocketCallBack
{
    public delegate void Callback(byte[] data);
    public event Callback callback;

    public void SendMessage(byte[] data){
        if (callback != null)
        {
            callback(data);
        }
    }
}
