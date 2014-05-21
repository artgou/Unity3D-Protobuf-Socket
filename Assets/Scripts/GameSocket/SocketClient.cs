using System;
using System.Net;
using System.Net.Sockets;

namespace GameSocket
{
    /// <summary>
    /// SocketClient操作控制类
    /// 
    /// <para>____________________________________________________________</para>
    /// <para>Version：V1.0.0</para>
    /// <para>Namespace：GameSocket</para>
    /// <para>Author: wboy    Time：2014/4/25 10:20:20</para>
    /// </summary>
    public class SocketClient
    {
        /// <summary>
        /// 最大收包缓存大小
        /// </summary>
        private const int bufferSize = 10240;
        private Socket connection;

        public event EventHandler<SocketMessageReceivedFromServer> SocketMessageReceivedFromServer;
        public event EventHandler<CreateConnectionAsyncArgs> CreateConnectCompleted;
        public event EventHandler CloseHandler;
        public event EventHandler ConnectError;
        public event EventHandler ReconnectHandle;

        /// <summary>
        /// 新建Socket连接
        /// </summary>
        /// <param name="serverAddress">server地址</param>
        /// <param name="port">端口号</param>
        public void CreateConnection(string serverAddress, int port)
        {
            if (this.connection != null && this.connection.Connected)
            {
                
            }

            this.connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.connection.NoDelay = true;
            IPEndPoint end_point = new IPEndPoint(IPAddress.Parse(serverAddress), port);
            this.connection.BeginConnect(end_point, new AsyncCallback(this.Connected), this.connection);
        }

        /// <summary>
        /// 销毁socket连接
        /// 
        /// <para>注意：在不需要使用时关闭socket连接</para>
        /// </summary>
        public void DisConnect()
        {
            if (this.connection != null && this.connection.Connected)
            {
                try
                {
                    this.connection.Shutdown(SocketShutdown.Both);
                    this.connection.Close();
                    this.connection = null;
                }
                catch
                {
                    this.connection = null;
                }
            }
        }

        /// <summary>
        /// 发送消息给server
        /// </summary>
        /// <param name="data"></param>
        public void SendMessageToServer(byte[] data)
        {
            if (this.connection == null)
            {
                if (this.ReconnectHandle != null)
                {
                    this.ReconnectHandle(this, new EventArgs());
                }
                return;
            }
            if (!this.connection.Connected)
            {
                if (this.ReconnectHandle != null)
                {
                    this.ReconnectHandle(this, new EventArgs());
                }
                return;
            }
            this.connection.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this.SendMessageToServerComplete), this.connection);
        }

        /// <summary>
        /// 信息发送到server完成回调
        /// </summary>
        /// <param name="iar"></param>
        private void SendMessageToServerComplete(IAsyncResult iar)
        {
            this.connection.EndSend(iar);
        }

        /// <summary>
        /// socket连接创建成功回调
        /// </summary>
        /// <param name="iar"></param>
        private void Connected(IAsyncResult iar)
        {
            try
            {
                if (this.CreateConnectCompleted != null)
                {
                    this.CreateConnectCompleted(this, new CreateConnectionAsyncArgs(true));
                }
                this.connection.EndConnect(iar);
                byte[] array = new byte[bufferSize];
                this.connection.BeginReceive(array, 0, bufferSize, SocketFlags.None, new AsyncCallback(this.KeepConnect), array);
            }
            catch(SocketException){
                if (ConnectError != null)
                {
                    ConnectError(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 保持socket连接, 一直监听server发过来的消息
        /// </summary>
        /// <param name="iar"></param>
        private void KeepConnect(IAsyncResult iar)
        {
            try
            {
                int num = this.connection.EndReceive(iar);
                if (num > 0)
                {
                    byte[] array = (byte[])iar.AsyncState;
                    if (this.SocketMessageReceivedFromServer != null && array != null)
                    {
                        this.SocketMessageReceivedFromServer(this, new SocketMessageReceivedFromServer(array, num));
                    }

                    array = new byte[bufferSize];
                    this.connection.BeginReceive(array, 0, bufferSize, SocketFlags.None, new AsyncCallback(this.KeepConnect), array);
                }
                else
                {
                    if (this.CloseHandler != null)
                    {
                        this.CloseHandler(this, new EventArgs());
                    }
                }
            }
            catch (SocketException)
            {
                
            }
        }
    }
}
