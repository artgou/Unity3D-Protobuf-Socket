using UnityEngine;
using System;
using GameSocket;
using System.Text;

public class Demo : MonoBehaviour {

    private SocketClient client;
    private SocketEventDispatcher diapatcher;

    public static int packetLength;

	// Use this for initialization
	void Start () {
        SocketCallBack callback = new SocketCallBack();
        callback.callback += GetServerMessage;
        DemoCMDProcess process = new DemoCMDProcess(callback);
        this.diapatcher = new SocketEventDispatcher(process);
        this.client = new SocketClient();
        this.client.SocketMessageReceivedFromServer += new EventHandler<SocketMessageReceivedFromServer>(this.OnReceiveMessageFromServer);
        this.client.CreateConnectCompleted += new EventHandler<CreateConnectionAsyncArgs>(this.OnCreateConnectionComplete);
        this.client.CloseHandler += new EventHandler(this.CloseHandler);
        this.client.ConnectError += new EventHandler(this.ConnectError);
	}

    // Update is called once per frame
    void Update()
    {
        this.diapatcher.IncomingData();
    }

    bool isConnnect = true;
    string inputTest = "Input your's value";
    string serverStr = string.Empty;
	
    void OnGUI()
    {
        if (isConnnect)
        {
            if (GUI.Button(new Rect(10, 10, 200, 50), "OpenConnect"))
            {
                
                this.client.CreateConnection("192.168.30.85", 2014);
            }
        }
        else
        {
            if (GUI.Button(new Rect(10, 10, 200, 50), "CloseConnect"))
            {
                

                this.client.DisConnect();
            }

            inputTest = GUI.TextField(new Rect(10, 100, 400, 20), inputTest);

            if (GUI.Button(new Rect(10, 200, 200, 50), "SendMessage"))
            {
                this.client.SendMessageToServer(StringToByteArr(inputTest));
            }

            GUI.Label(new Rect(10, 300, 500, 20), "SeverCallBack : " + serverStr);
        }
    }

    private void OnCreateConnectionComplete(object sender, CreateConnectionAsyncArgs e)
    {
        isConnnect = false;
    }

    private void OnReceiveMessageFromServer(object sender, SocketMessageReceivedFromServer e)
    {
        Debug.Log("go to this OnReceiveMessageFromServer");
        this.diapatcher.AddData(e.Message, e.BytesTransferred);
    }

    private void ReconnectHandler(object sender, EventArgs e)
    {
        
    }

    private void CloseHandler(object sender, EventArgs e)
    {
        isConnnect = true;
    }

    private void ConnectError(object sender, EventArgs e)
    {

    }

    private void GetServerMessage(byte[] bytes)
    {
        Debug.Log("go to this GetServerMessage");
        this.serverStr = ByteArrToString(bytes);
    }


    private byte[] StringToByteArr(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        packetLength = bytes.Length;
        return bytes;
    }

    private string ByteArrToString(byte[] bytes)
    {
        string str = Encoding.UTF8.GetString(bytes);
        return str;
    }
}
