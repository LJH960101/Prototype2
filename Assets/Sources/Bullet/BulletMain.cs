using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletMain : NetworkBehaviour {
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
        initVelocity = transform.GetComponent<Rigidbody2D>().velocity;
        initPos = MyTool.GetPlayerGameObject(_bulletTargetPlayer).transform.position;
        transform.position = initPos;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
                transform.GetComponent<Rigidbody2D>().velocity = initVelocity;
                onLerp = false;
            }
        }
    }
}
