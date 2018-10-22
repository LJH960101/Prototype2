using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMain : NetworkBehaviour {
    GameObject _myAim = null;
    [SyncVar]
    int _playerId = -1;
    public int PlayerId { get { return _playerId; } }
    public override void OnStartLocalPlayer()
    {
        _myAim = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerAim"));
    }
    private void OnDestroy()
    {
        if (_myAim != null)
        {
            Destroy(_myAim);
            _myAim = null;
        }
    }

    // Use this for initialization
    void Start () {
        _playerId = GameObject.FindGameObjectsWithTag("Player").Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
