using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletMain : NetworkBehaviour {
    [SyncVar]
    int _bulletTargetPlayer = -1;
    public int BulletTargetPlayer
    {
        get { return _bulletTargetPlayer; }
        set { _bulletTargetPlayer = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
