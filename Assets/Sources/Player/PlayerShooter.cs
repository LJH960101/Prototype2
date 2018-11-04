using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooter : NetworkBehaviour
{
    GameObject _aimObj;
    PlayerConstant _pc;
    PlayerAnimation _pa;
    PlayerMain _pm;
    float shootTimer = 0.0f;
    int _shootCount = 0;
    // Use this for initialization
    void Start () {
        _aimObj = GameObject.FindGameObjectWithTag("Aim");
        _pc = GetComponent<PlayerConstant>();
        _pm = GetComponent<PlayerMain>();
        _pa = transform.Find("Model").GetComponent<PlayerAnimation>();
	}
    bool onMouseDown = false;
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        shootTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0)) onMouseDown = true;
        if (Input.GetMouseButtonUp(0)) onMouseDown = false;
        if (_aimObj == null) _aimObj = GameObject.FindGameObjectWithTag("Aim");
        if(_aimObj != null)
        {
            if (onMouseDown && _pm.attackAble && shootTimer <= 0.0f)
            {
                Vector3 shootVec3D = (_aimObj.transform.position - transform.position);
                shootVec3D.z = 0f;
                Vector2 shootVec = shootVec3D.normalized;
                preShot(_pm.PlayerId, shootVec, transform.position);
                CmdShoot(_pm.PlayerId, shootVec, transform.position);
                shootTimer = _pc.ShootDelay;
            }
        }
	}
    void preShot(int playerId, Vector2 shootForce, Vector3 startPos)
    {
        if (isServer) return;
        _pa.RunShootAnimation();
        var bulletObj = Instantiate(_pc.Bullet);
        bulletObj.transform.position = startPos;
        bulletObj.GetComponent<BulletMain>().BulletTargetPlayer = playerId;
        bulletObj.GetComponent<Rigidbody>().velocity = shootForce * _pc.ShootPower * 0.8f;
        bulletObj.GetComponent<BulletMain>().damage = _pc.Damage;
        Destroy(bulletObj, 10.0f);
    }
    [Command]
    void CmdShoot(int playerId, Vector2 shootForce, Vector3 startPos)
    {
        _pa.RunShootAnimation();
        var bulletObj = Instantiate(_pc.Bullet);
        bulletObj.transform.position = startPos;
        bulletObj.GetComponent<BulletMain>().BulletTargetPlayer = playerId;
        bulletObj.GetComponent<Rigidbody>().velocity = shootForce * _pc.ShootPower;
        bulletObj.GetComponent<BulletMain>().damage = _pc.Damage;
        Destroy(bulletObj, 10.0f);
        RpcShoot(playerId, shootForce, startPos);
    }
    [ClientRpc]
    void RpcShoot(int playerId, Vector2 shootForce, Vector3 startPos)
    {
        if (isServer) return;
        _pa.RunShootAnimation();
        if (playerId == MyTool.GetLocalPlayer().PlayerId) return;
        var bulletObj = Instantiate(_pc.Bullet);
        bulletObj.transform.position = startPos;
        bulletObj.GetComponent<BulletMain>().BulletTargetPlayer = playerId;
        bulletObj.GetComponent<Rigidbody>().velocity = shootForce * _pc.ShootPower;
        bulletObj.GetComponent<BulletMain>().damage = _pc.Damage;
        Destroy(bulletObj, 10.0f);
        Debug.Log(3);
    }

    [ClientRpc]
    void RpcShootAnim()
    {
        _pa.RunShootAnimation();
    }
}
