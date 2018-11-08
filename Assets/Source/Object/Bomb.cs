using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour {
    // Use this for initialization
    [SerializeField]
    float speed = 35.0f;
    bool bGoRight;
    [SyncVar, HideInInspector]
    public int ownPlayerId;
    [SyncVar(hook ="OnFlip")]
    bool onFlip = false;
    void OnFlip(bool onFlip)
    {
        if(onFlip) GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;
    }
    public bool InitPos()
    {
        // 왼쪽 출몰
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            transform.position = new Vector3(-55, transform.position.y, -5f);
            GetComponent<Rigidbody>().velocity = new Vector3(speed, 0.0f, 0.0f);
            bGoRight = true;
        }
        else
        {
            transform.position = new Vector3(55, transform.position.y, -5f);
            GetComponent<Rigidbody>().velocity = new Vector3(-speed, 0.0f, 0.0f);
            onFlip = true;
            OnFlip(onFlip);
            bGoRight = false;
        }
        return bGoRight;
    }
	// Update is called once per frame
	void Update () {
        if (!isServer) return;
        if (bGoRight)
        {
            if (transform.position.x >= 65) Destroy(gameObject);
        }
        else
        {
            if (transform.position.x <= -65) Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if(other.transform.tag == "Player")
        {
            if(other.GetComponent<PlayerMain>().PlayerId%2 != ownPlayerId % 2)
            {
                other.GetComponent<PlayerMain>().CmdDie();
            }
        }
        else if(other.transform.tag == "Monster")
        {
            other.transform.GetComponent<Monster>().GetDamage(10000, ownPlayerId);
        }
    }
}
