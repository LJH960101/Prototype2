using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour {
	// Use this for initialization
	void Start () {
        FindObjectOfType<PlayerCamera>().SetPlayerAim(this);
    }
	
	// Update is called once per frame
	void Update()
    {
        var v3 = Input.mousePosition;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        v3.z = -5f;
        transform.position = v3;
    }
}
