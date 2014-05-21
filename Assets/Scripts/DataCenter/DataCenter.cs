/*
================================================================================
FileName    : DataCenter
Description : 网络数据包的相关的类统一管理与调用
Date        : 2014-05-09
Author      : Linkrules
================================================================================
*/
using UnityEngine;
using System.Collections;
using GamePacketData;
public class DataCenter {

    static private ProtobufUtility _protobufUtility = null;
    static public ProtobufUtility ProtobufUtility {
        get {
            if ( _protobufUtility == null ) {
                _protobufUtility = new ProtobufUtility();
            }
            return _protobufUtility;
        }
    }


    static private PacketBuilder _packetBuilder = null;
    static public PacketBuilder PacketBuilder {
        get {
            if ( _packetBuilder == null ) {
                _packetBuilder = new PacketBuilder();
            }
            return _packetBuilder;
        }
    }


    static private PacketParser _packetParser = null;
    static public PacketParser PacketParser {
        get {
            if ( _packetParser == null ) {
                _packetParser = new PacketParser();
            }
            return _packetParser;
        }
    }

    static private PacketProcesser _packetProcesser = null;
    static public PacketProcesser packetProcesser {
        get {
            if ( _packetProcesser == null ) {
                _packetProcesser = new PacketProcesser();
            }
            return _packetProcesser;
        }
    }


    static private GameClient _gameClient = null;
    static public GameClient gameClient {
        get {
            if ( _gameClient == null ) {
                GameObject obj = new GameObject();
                obj.name = "GameClient";
                _gameClient = obj.AddComponent<GameClient>();
                Object.DontDestroyOnLoad( obj );
                _gameClient.InitClient();
            }
            return _gameClient;
        }
    }






}
