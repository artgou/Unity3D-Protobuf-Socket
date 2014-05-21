/*
================================================================================
FileName    : GameClient
Description : 实例化单一 SocketClient ， 功能：连接与断开服务器，发送，接收，派发消息
Date        : 2014-05-09
Author      : Linkrules
================================================================================
*/
using UnityEngine;
using System.Collections;
using System;
using GameSocket;

public class GameClient : MonoBehaviour{
    private SocketClient client = null;
    private SocketEventDispatcher dispatcher;

    private bool isConnected = false;
    


    void Update() {
        this.dispatcher.IncomingData();
    }

    


    private void OnCreateConnectionComplete( object sender, CreateConnectionAsyncArgs e ) {
        isConnected = true;
    }

    public void OnReceiveMessageFromServer(object sender,SocketMessageReceivedFromServer e) {
        this.dispatcher.AddData( e.Message, e.BytesTransferred );
    }

    private void CloseHandler( object sender, EventArgs e ) {
        isConnected = false;
    }

    private void ConnectError( object sender, EventArgs e ) {

    }


    public void InitClient() {
        SocketCallBack callback = new SocketCallBack();
        callback.callback += GetServerMessage;
        DemoCMDProcess process = new DemoCMDProcess( callback );
        this.dispatcher = new SocketEventDispatcher( process );
        this.client = new SocketClient();
        this.client.SocketMessageReceivedFromServer += new System.EventHandler<SocketMessageReceivedFromServer>( this.OnReceiveMessageFromServer );
        this.client.CreateConnectCompleted += new EventHandler<CreateConnectionAsyncArgs>( this.OnCreateConnectionComplete );
        this.client.CloseHandler += new EventHandler( this.CloseHandler );
        this.client.ConnectError += new EventHandler( this.ConnectError );
    }


    public void ConnectToServer( string ip, int port ) {
        if ( this.client == null ) {
            InitClient();
        }
        this.client.CreateConnection( ip, port );
    }


    /// <summary>
    /// 当消息来自服务器后，此函数被DemoCMDProcess 回调，数据包被传入解析并派发
    /// </summary>
    /// <param name="data">一个完整的消息数据包</param>
    private void GetServerMessage(byte[] data) {
        DataCenter.PacketParser.Parse( data );
    }


    /// <summary>
    /// 将定义好的 protobuf 消息对象发给服务器
    /// </summary>
    /// <param name="messageObject"></param>
    public void SendMessage( object messageObject ) {
        byte[] messageBytes = DataCenter.PacketBuilder.Build( messageObject );
        this.client.SendMessageToServer( messageBytes );
    }

	
}
