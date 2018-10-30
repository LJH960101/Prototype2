using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FallDown : NetworkBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.transform.tag == "Player")
        {
            other.transform.GetComponent<PlayerMain>().CmdDie();
        }
    }
}
