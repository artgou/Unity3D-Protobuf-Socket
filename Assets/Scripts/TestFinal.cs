using UnityEngine;
using System.Collections;
using com.CR.GameDataModel;
using System.Text;
using System;
using System.Runtime.InteropServices;

public class TestFinal : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //DataCenter.gameClient.ConnectToServer( "192.168.1.1", 2014 );
        int uvariable = 12;

        Debug.Log( Marshal.SizeOf( uvariable ) );
        
	}
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetKeyDown( KeyCode.C ) ) {
            Attack attack = new Attack();
            attack.hurtValue = 1000;
            attack.weaponId = 60;
            attack.weaponName = "Weapon 001";
            DataCenter.gameClient.SendMessage(attack);

        }
	
	}
}
