using UnityEngine;
using System.Collections;
using com.CR.GameDataModel;
using System;
using System.Text;

public class Test : MonoBehaviour {
    void Start() {
        DataCenter.packetProcesser.event_Attack += Callback_Attack;
        DataCenter.packetProcesser.event_Login += Callback_Login;
        DataCenter.packetProcesser.event_Player += Callback_Player;

       

    }

    void OnDiable() {
        DataCenter.packetProcesser.event_Attack -= Callback_Attack;
        DataCenter.packetProcesser.event_Login -= Callback_Login;
        DataCenter.packetProcesser.event_Player -= Callback_Player;
    }

    void Callback_Login(Login message) {
        Debug.Log( message.loginName + "    " + Time.time);
    }

    void Callback_Player( Player player ) {
        Debug.Log(player.name + "    " + Time.time);
    }

    void Callback_Attack( Attack attack ) {
        Debug.Log( attack.weaponName + "    " + attack.hurtValue );
    }

  

  


}
