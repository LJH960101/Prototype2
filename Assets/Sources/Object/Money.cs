using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Money : NetworkBehaviour
{
    int moneyValue = 30;
    int bigMoneyValue = 70;
    bool isBig;

    GameObject drop, money;
    private void Start()
    {
        drop = transform.Find("DropImage").gameObject;
        money = transform.Find("MoneyImage").gameObject;
        money.SetActive(false);
        drop.SetActive(true);

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
    private void OnCollisionEnter(Collision collision)
    {
        money.SetActive(true);
        drop.SetActive(false);
    }
    [SerializeField]
    AudioClip popCornGet;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isServer)
            {
                if(isBig) other.gameObject.GetComponent<PlayerMain>().CmdAddMoney(bigMoneyValue);
                else other.gameObject.GetComponent<PlayerMain>().CmdAddMoney(moneyValue);
            }
            MyTool.GetLocalPlayer().GetComponent<AudioSource>().PlayOneShot(popCornGet);
            Destroy(gameObject);
        }

    }
}
