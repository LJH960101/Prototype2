using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Money : NetworkBehaviour {
    public int moneyValue = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(isServer) other.gameObject.GetComponent<PlayerMain>().CmdAddMoney(moneyValue);
            Destroy(gameObject);
        }

    }
}
