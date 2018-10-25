using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Money : NetworkBehaviour {
    public int moneyValue = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMain>().CmdAddMoney(moneyValue);
            Destroy(gameObject);
        }

    }
}
