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
        if (UnityEngine.Random.Range(0, 10) == 1)
        {
            isBig = true;
            transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
        else isBig = false;
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
