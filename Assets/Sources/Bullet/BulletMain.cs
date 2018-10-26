using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletMain : NetworkBehaviour {
    public int damage = 10;
    public int forcePower = 100;
    [SyncVar]
    int _bulletTargetPlayer = -1;
    public int BulletTargetPlayer
    {
        get { return _bulletTargetPlayer; }
        set { _bulletTargetPlayer = value; }
    }
    Vector3 realPos;
    Vector3 targetPos;
    Vector3 initVelocity;
    Vector3 initPos;
    bool onLerp;
    float lerpCounter;
    const float lerpTime = 1f;

	// Use this for initialization
	void Start () {
        realPos = transform.position;
        initVelocity = transform.GetComponent<Rigidbody>().velocity;
        initPos = MyTool.GetPlayerGameObject(_bulletTargetPlayer).transform.position;
        transform.position = initPos;
        transform.GetComponent<Rigidbody>().velocity = Vector2.zero;
        targetPos = realPos + initVelocity * lerpTime;
        onLerp = true;
        lerpCounter = 0.0f;
    }

    private void Update()
    {
        if (onLerp)
        {
            realPos += initVelocity * Time.deltaTime;
            lerpCounter = Mathf.Clamp(lerpCounter + Time.deltaTime, 0.0f, lerpTime + 0.1f);
            transform.position = Vector2.Lerp(initPos, targetPos, lerpCounter / lerpTime); 

            // 보간 끝
            if (lerpCounter >= lerpTime)
            {
                transform.position = realPos;
                transform.GetComponent<Rigidbody>().velocity = initVelocity;
                onLerp = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.transform.tag == "Player")
        {
            if (BulletTargetPlayer % 2 == other.gameObject.GetComponent<PlayerMain>().PlayerId % 2)
            {
                return;
            }
            Vector3 bulletForce = initVelocity.normalized * damage/2f * forcePower;
            bulletForce.y = bulletForce.y * 0.01f;
            other.gameObject.GetComponent<PlayerMain>().CmdGetDamage(damage);
            other.gameObject.GetComponent<Rigidbody>().AddForce(bulletForce);
            Destroy(gameObject);
        }
        else if (other.transform.tag == "Monster")
        {
            Vector3 bulletForce = initVelocity.normalized * damage * forcePower;
            other.gameObject.GetComponent<Monster>().GetDamage(damage, _bulletTargetPlayer);
            other.gameObject.GetComponent<Rigidbody>().AddForce(bulletForce);
            Destroy(gameObject);
        }
        else if (other.transform.tag == "Bullet") return;
        else
        {
            Destroy(gameObject);
        }
    }
}
