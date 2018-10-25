using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FallDown : NetworkBehaviour {
    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerMain>().CmdDie();
        }
    }
}
