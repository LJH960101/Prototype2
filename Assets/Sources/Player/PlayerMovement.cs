using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    PlayerConstant _pc;
    PlayerNetwork _pn;
    PlayerMain _pm;
    PlayerAnimation _pa;
    Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _pc = GetComponent<PlayerConstant>();
        _pn = GetComponent<PlayerNetwork>();
        _pm = GetComponent<PlayerMain>();
        _pa = transform.Find("Model").GetComponent<PlayerAnimation>();
    }
    void Jump()
    {
        _rb.AddForce(new Vector2(0f, _pc.JumpPower));
    }
    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer)
            return;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && !_pa.OnJump) Jump();
        if (Input.GetKeyDown(KeyCode.Alpha1)) BuyItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) BuyItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) BuyItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) BuyItem(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) BuyItem(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) _pm.CmdAddMoney(10000);
    }

    [Command]
    void CmdAddDamage()
    {
        _pc.Damage += _pc.ItemDamage;
    }
    [Command]
    void CmdAddSpeed()
    {
        _pc.Speed += _pc.ItemSpeed;
        Invoke("BackToOriginSpeed", _pc.ItemTime);
    }
    void BackToOriginSpeed()
    {
        if (gameObject == null) return;
        _pc.Speed -= _pc.ItemSpeed;
    }
    [Command]
    void CmdAddAttackSpeed()
    {
        _pc.ShootDelay -= _pc.ItemAttackSpeed;
        Invoke("BackToOriginAttackSpeed", _pc.ItemTime);
    }
    void BackToOriginAttackSpeed()
    {
        if (gameObject == null) return;
        _pc.ShootDelay += _pc.ItemAttackSpeed;
    }
    [Command]
    void CmdSpawnBomb()
    {
        GameObject bomb = Instantiate(_pc.Bomb, transform.position, Quaternion.identity);
        bomb.GetComponent<Bomb>().ownPlayerId = _pm.PlayerId;
        bool pos = bomb.GetComponent<Bomb>().InitPos();
        NetworkServer.Spawn(bomb);
        RpcSpawnCutScene(pos);
        SpawnCutScene(pos);
    }
    [SerializeField]
    GameObject right, left;
    [ClientRpc]
    void RpcSpawnCutScene(bool goRight)
    {
        SpawnCutScene(goRight);
    }
    void SpawnCutScene(bool goRight)
    {
        var canvas = GameObject.Find("Canvas").transform;
        if (!goRight) Instantiate(right, canvas);
        else Instantiate(left, canvas);
    }
    void BuyItem(int stuffCode)
    {
        Stuff stuff = MyTool.GetStuff(stuffCode);

        if (stuff == null) Debug.LogError("Not exist stuffCode");
        if (!_pm.attackAble && stuff.stuffType == Stuff.StuffType.BOMB) return;
        if (stuff.GetPrice() < _pm.Money)
        {
            _pm.CmdAddMoney(-stuff.GetPrice());
            bool successToUse = stuff.Use();
            if (successToUse)
            {
                switch (stuff.stuffType)
                {
                    case Stuff.StuffType.ATTACK_SPEED:
                        CmdAddAttackSpeed();
                        break;
                    case Stuff.StuffType.BOMB:
                        CmdSpawnBomb();
                        break;
                    case Stuff.StuffType.HP:
                        _pm.CmdAddMaxHp();
                        break;
                    case Stuff.StuffType.POWER:
                        CmdAddDamage();
                        break;
                    case Stuff.StuffType.SPEED:
                        CmdAddSpeed();
                        break;
                }
            }
        }
    }

    [Command]
    void CmdMoveToPortal()
    {
        _pm.CmdMoveTo(GameObject.FindGameObjectWithTag("Portal").transform.position);
    }
    private void FixedUpdate()
    {
        if (isServer && (transform.position.x <= -39 || transform.position.x >= 37))
        {
            CmdMoveToPortal();
        }
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * 1f * _pc.Speed;

        _rb.velocity = new Vector2(x, _rb.velocity.y);
    }
}
