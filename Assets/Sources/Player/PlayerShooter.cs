﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooter : NetworkBehaviour
{
    GameObject _aimObj;
    PlayerConstant _pc;
    // Use this for initialization
    void Start () {
        _aimObj = GameObject.FindGameObjectWithTag("Aim");
        _pc = GetComponent<PlayerConstant>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if(_aimObj == null) _aimObj = GameObject.FindGameObjectWithTag("Aim");
        if(_aimObj != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 shootVec3D = (_aimObj.transform.position - transform.position);
                shootVec3D.z = 0f;
                Vector2 shootVec = shootVec3D.normalized;
                CmdShoot(GetComponent<PlayerMain>().PlayerId, shootVec, transform.position);
            }
        }
	}

    [Command]
    void CmdShoot(int playerId, Vector2 shootForce, Vector3 startPos)
    {
        var bulletObj = Instantiate(_pc.Bullet);
        bulletObj.transform.position = startPos;
        bulletObj.GetComponent<BulletMain>().BulletTargetPlayer = playerId;
        bulletObj.GetComponent<Rigidbody2D>().velocity = shootForce * _pc.ShootPower;
        NetworkServer.Spawn(bulletObj);
        Destroy(bulletObj, 2.0f);
    }
}
