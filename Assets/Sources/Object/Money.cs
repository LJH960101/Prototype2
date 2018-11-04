using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Money : NetworkBehaviour
{
    int moneyValue = 30;
    int bigMoneyValue = 70;
    bool isBig;
    private void Start()
    {
        if (!isServer) return;
        if (UnityEngine.Random.Range(0, 10) == 1)
        {
            isBig = true;
            transform.localScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, transform.localScale.z * 2f);
            RpcSetIsBig();
        }
        else isBig = false;
    }
    [ClientRpc]
    public void RpcSetIsBig()
    {
        isBig = true;
        transform.localScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, transform.localScale.z * 2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isServer)
            {
                if(isBig) other.gameObject.GetComponent<PlayerMain>().CmdAddMoney(bigMoneyValue);
                else other.gameObject.GetComponent<PlayerMain>().CmdAddMoney(moneyValue);
            }
            Destroy(gameObject);
        }

    }
}
