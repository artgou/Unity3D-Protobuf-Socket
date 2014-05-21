using System;
using GameSocket;
using UnityEngine;


/// <summary>
/// DemoCMDProcess 的摘要说明
/// 
/// <para>____________________________________________________________</para>
/// <para>Version：V1.0.0</para>
/// <para>Namespace：GameSocket</para>
/// <para>Author: wboy    Time：2014/4/25 12:32:18</para>
/// </summary>
public class DemoCMDProcess : IProcessCMD
{
    private const int packetMaxLength = 10240;
    private SocketCallBack mCallback;

    private int writeIndex = 0;
    private byte[] buffer;

    public DemoCMDProcess(SocketCallBack Callback)
    {
        this.buffer = new byte[packetMaxLength];
        this.mCallback = Callback;
    }

    /*
    override public void IncomingData(byte[] data, int actualSize)
    {
        if (this.writeIndex + actualSize >= packetMaxLength)
        {
            throw new Exception("Buffer Overflow!");
        }
        Array.Copy(data, 0, this.buffer, this.writeIndex, actualSize);
        this.writeIndex += actualSize;
        while (this.writeIndex >= Demo.packetLength)
        {
            byte[] bytes = new byte[Demo.packetLength];
            Array.Copy(this.buffer, 0, bytes, 0, Demo.packetLength);
            this.mCallback.SendMessage(bytes);
            this.writeIndex = 0;
        }
    }
    */

    override public void IncomingData( byte[] data, int actualSize ) {
        if( this.writeIndex + actualSize >= packetMaxLength ) {
            throw new Exception( "Buffer Overflow!" );
        }
        Array.Copy( data, 0, this.buffer, this.writeIndex, actualSize );
        this.writeIndex += actualSize;

        if( writeIndex < 1 ) {
            return;
        }

        int len = BitConverter.ToUInt16( this.buffer, 0 );
        Debug.Log( "writeIndex: " + writeIndex + "    len: " + len + "    " + data.Length + "------");
       
        
        while ( this.writeIndex >= 1 && this.writeIndex >= len - 1 ) {
            byte[] result = new byte[len];
            Array.Copy( this.buffer, 0, result, 0, len );
            this.buffer = Remove( this.buffer, ref this.writeIndex, len );
            this.mCallback.SendMessage( result );
            len = BitConverter.ToUInt16( this.buffer, 0 );
            Debug.Log( "writeIndex: " + writeIndex + "    len: " + len + "    " + data.Length + "------In While");
        }

    }


    private byte[] Remove( byte[] data, ref int writeIndex, int len ) {
        byte[] newBuffer = new byte[packetMaxLength];
        Array.Copy( data, len, newBuffer, 0, writeIndex + 1 - len );
        writeIndex -= len;
        return newBuffer;
    }
}
