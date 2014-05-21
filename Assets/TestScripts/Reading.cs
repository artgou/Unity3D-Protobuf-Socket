using UnityEngine;
using System.Collections;
using com.CR.GameDataModel;
using System.IO;
using System.Text;
using JsonFx.Json;
public class Reading : MonoBehaviour {
	// Use this for initialization
    public bool isReadyProtobuf = false;
    public bool isReadyString = false;
    public bool isReadyJson = false;
	void Start () {
       // WriteFile( LoadFile() );
	}

    void Update() {
        if ( isReadyProtobuf ) {
            object obj = ConstructSourceData();
            SaveData( obj );
            LoadData();
            isReadyProtobuf = false;
        }

        if ( isReadyString ) {
            object obj = ConstructSourceData();
            SaveData( obj, 1 );
            LoadData( 1 );
            isReadyString = false;
        }

        if ( isReadyJson ) {
            object obj = ConstructSourceDataJson();
            JsonDataSave( obj );
            JsonDataLoad();
            isReadyJson = false;
        }
    }

    object ConstructSourceData() {
        Player player = new Player();
        player.id = 10000;
        player.name = "Robot 001";
        player.level = 5000;
        player.golds = 152420;
        return player;
    }

    object ConstructSourceDataJson() {
        Player2 player2 = new Player2();
        player2.id = 10001;
        player2.name = "test 001";
        player2.level = 185;
        player2.golds = 1234567;
        player2.attackValue = 5242;

        return player2;
    }

    void LoadData() {
        float startTime = Time.realtimeSinceStartup;
        string file = Application.streamingAssetsPath + "/GameData.dat";
        Stream stream = new FileStream( file, FileMode.Open );
        BinaryReader sr = new BinaryReader( stream, Encoding.Unicode );
        byte[] buffer = sr.ReadBytes( (int)stream.Length );
        Player obj = (Player)DataCenter.ProtobufUtility.Deserialize( buffer,"Player" );
        float useTime = Time.realtimeSinceStartup - startTime;
        sr.Close();
        stream.Close();
        Debug.Log( "Protobuf LoadData: " + useTime );
        Debug.Log( obj.id + "    " + obj.name + "    " + obj.level + "    " + obj.golds );
    }

    void SaveData(object obj) {
        float startTime = Time.realtimeSinceStartup;
        string file = Application.streamingAssetsPath + "/GameData.dat";
        Stream stream = new FileStream( file, FileMode.Create );
        BinaryWriter sw = new BinaryWriter( stream, Encoding.Unicode );
        byte[] buffer = DataCenter.ProtobufUtility.Serialize( obj );
        sw.Write( buffer );
        sw.Close();
        stream.Close();
        float useTime = Time.realtimeSinceStartup - startTime;
        Debug.Log( "Protobuf SaveData: " + useTime );
    }

    void LoadData( int s ) {
        float startTime = Time.realtimeSinceStartup;
        string file = Application.streamingAssetsPath + "/GameData2.dat";
        Stream stream = new FileStream( file, FileMode.Open );
        BinaryReader sr = new BinaryReader( stream, Encoding.Unicode );
        byte[] buffer = sr.ReadBytes( (int)stream.Length );

        string str = Encoding.UTF8.GetString( buffer );
        string[] sourceData = str.Split( ':' );
        Player obj = new Player();
        obj.id = int.Parse( sourceData[0] );
        obj.name = sourceData[1];
        obj.level = int.Parse(sourceData[2]);
        obj.golds = int.Parse( sourceData[3] );

        float useTime = Time.realtimeSinceStartup - startTime;
        sr.Close();
        stream.Close();
        Debug.Log( "String LoadData: " + useTime );
        Debug.Log( obj.id + "    " + obj.name + "    " + obj.level + "    " + obj.golds );
    }

    void SaveData(object obj, int s ) {
        float startTime = Time.realtimeSinceStartup;
        string file = Application.streamingAssetsPath + "/GameData2.dat";
        Stream stream = new FileStream( file, FileMode.Create );
        BinaryWriter sw = new BinaryWriter( stream, Encoding.Unicode );

        Player player = (Player)obj;
        string data = string.Format( "{0}:{1}:{2}:{3}", player.id, player.name, player.level, player.golds );
        byte[] buffer = Encoding.UTF8.GetBytes( data );

        sw.Write( buffer );
        sw.Close();
        stream.Close();
        float useTime = Time.realtimeSinceStartup - startTime;
        Debug.Log( "String SaveData: " + useTime );
    }

   
    

    void JsonDataLoad() {
        float startTime = Time.realtimeSinceStartup;
        string file = Application.streamingAssetsPath + "/GameData3.dat";
        Stream stream = new FileStream( file, FileMode.Open );
        BinaryReader sr = new BinaryReader( stream, Encoding.Unicode );
        byte[] buffer = sr.ReadBytes( (int)stream.Length );

        string str = Encoding.UTF8.GetString( buffer );
        Player2 players = JsonReader.Deserialize<Player2>( str );

        float useTime = Time.realtimeSinceStartup - startTime;
        sr.Close();
        stream.Close();
        Debug.Log( "String LoadData: " + useTime );
        Debug.Log( players.name );
        
    }

    void JsonDataSave(object obj) {

        string data = null;
        data = JsonWriter.Serialize( obj );

        float startTime = Time.realtimeSinceStartup;
        string file = Application.streamingAssetsPath + "/GameData3.dat";
        Stream stream = new FileStream( file, FileMode.Create );
        BinaryWriter sw = new BinaryWriter( stream, Encoding.Unicode );

        byte[] buffer = Encoding.UTF8.GetBytes( data );

        sw.Write( buffer );
        sw.Close();
        stream.Close();
        float useTime = Time.realtimeSinceStartup - startTime;
        Debug.Log( "String SaveData: " + useTime );

    }


    class Player2 {
        public int id = 0;
        public string name = "";
        public int level = 0;
        public int golds = 0;
        public int attackValue = 0;

        public Player2() { }
    }




}
