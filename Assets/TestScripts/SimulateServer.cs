using UnityEngine;
using System.Collections;
using com.CR.GameDataModel;
using GameSocket;
using System;

public class SimulateServer : MonoBehaviour {

    // Use this for initialization
    void Start() {
        StartCoroutine( SendMessageToClient() );

    }

    IEnumerator SendMessageToClient() {
        Player player = new Player();
        player.id = 1000;
        player.name = "Jack";
        player.level = 5;
        player.golds = 150;

        byte[] data = DataCenter.PacketBuilder.Build( player );
        SocketMessageReceivedFromServer smrf = new SocketMessageReceivedFromServer(data,data.Length);
        DataCenter.gameClient.OnReceiveMessageFromServer( this, smrf );
        Debug.Log( "SendMessageToClient: data.length: " + data.Length );
        yield return new WaitForSeconds( 2.0f );

        Login login = new Login();
        login.loginName = "Tony";
        login.loginPassword = "JJCCJDJD";
        data = DataCenter.PacketBuilder.Build( login );

        Attack attack = new Attack();
        attack.weaponId = 1234;
        attack.weaponName = "RPG";
        attack.hurtValue = 1530;
        byte[] data2 = DataCenter.PacketBuilder.Build( attack );
        Debug.Log( "Attack: " + data2.Length );
        /*
        smrf = new SocketMessageReceivedFromServer( data2, data2.Length );
        DataCenter.gameClient.OnReceiveMessageFromServer( this, smrf );
        */
        
        byte[] finalData = new byte[data.Length + data2.Length];
        int writeIndex = 0;
        Array.Copy( data, 0, finalData, writeIndex, data.Length );
        writeIndex += data.Length;
        Array.Copy( data2, 0, finalData, writeIndex, data2.Length );
        smrf = new SocketMessageReceivedFromServer( finalData, finalData.Length );
        DataCenter.gameClient.OnReceiveMessageFromServer( this, smrf );
        yield return new WaitForSeconds( 2.0f );

        int firstLen = data2.Length / 2;
        byte[] bytes = new byte[firstLen];
        Array.Copy( data2, 0, bytes, 0, firstLen );
        smrf = new SocketMessageReceivedFromServer( bytes, bytes.Length );
        DataCenter.gameClient.OnReceiveMessageFromServer( this, smrf );
        yield return new WaitForSeconds( 1.0f );
        Debug.Log( "Another half data is ready---------------------------" );
        bytes = new byte[data2.Length - firstLen];
        Array.Copy( data2, firstLen, bytes, 0, data2.Length - firstLen );
        smrf = new SocketMessageReceivedFromServer( bytes, bytes.Length );
        DataCenter.gameClient.OnReceiveMessageFromServer( this, smrf );


        
    }
    
}