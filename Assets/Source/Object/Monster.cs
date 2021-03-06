﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Monster : NetworkBehaviour
{
    [SerializeField]
    Transform hpBar;
    public float speed = 1f;
    public int _maxHp = 100;
    public int _rewordMoney = 100;
    [SyncVar(hook = "OnHpChange")]
    int _hp;
    SpriteRenderer _sr;
    [SyncVar(hook = "OnFlip")]
    bool onFlip = false;
    Rigidbody _rb;
    void OnFlip(bool flip)
    {
        if (!isServer) onFlip = flip;
        ChangeFlipState();
    }
    void ChangeFlipState()
    {
        _sr.flipX = onFlip;
        move.GetComponent<SpriteRenderer>().flipX = onFlip;
        attack.GetComponent<SpriteRenderer>().flipX = onFlip;
    }
    public override void OnStartClient()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    void OnHpChange(int hp)
    {
        if (!isServer) _hp = hp;
        RefreshHp();
    }
    void RefreshHp()
    {
        hpBar.localScale = new Vector3((float)_hp / (float)_maxHp, 1.0f, 1.0f);
        hpBar.localPosition = new Vector3(-(1.0f - ((float)_hp / (float)_maxHp)), 0.0f, 0.0f);
    }

    List<Transform> playerTransforms;
    Transform targetTransform = null;
    // Use this for initialization
    GameObject move, attack;
    [SerializeField]
    AudioClip startSound, dieSound;
    AudioSource _as;
	void Start () {
        _as = GetComponent<AudioSource>();
        _as.PlayOneShot(startSound);
        playerTransforms = new List<Transform>();
        var players = GameObject.FindObjectsOfType<PlayerMain>();
        foreach (var player in players) playerTransforms.Add(player.transform);
        _hp = _maxHp;
        _rb = GetComponent<Rigidbody>();
        MyTool.GetLocalPlayer().AddMonster(this);
        MyTool.GetLocalPlayer().CalcPos(this);
        RefreshHp();
        move = transform.Find("MonsterMove").gameObject;
        attack = transform.Find("MonsterAttack").gameObject;
        move.SetActive(true);
        attack.SetActive(false);
    }
    [ClientRpc]
    void RpcDie()
    {
        Die();
    }
    bool onDie = false;
    void Die()
    {
        onDie = true;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Monster>().enabled = false;
        transform.Find("MonsterMove").gameObject.SetActive(false);
        transform.Find("MonsterAttack").gameObject.SetActive(false);
        transform.Find("HPFrame").gameObject.SetActive(false);
        _as.PlayOneShot(dieSound);
        Instantiate(effect1, transform.position, Quaternion.Euler(new Vector3(-90, 0f, 0f)));
        Instantiate(effect2, transform.position, Quaternion.Euler(new Vector3(-90, 0f, 0f)));
        MyTool.GetLocalPlayer().RemoveMonster(this);
    }
    public GameObject effect1, effect2;

    public void GetDamage(int damage, int bulletShooterCode)
    {
        _hp -= damage;
        if (_hp <= 0 && isServer)
        {
            MyTool.GetPlayerGameObject(bulletShooterCode).GetComponent<PlayerNetwork>().CmdAddScore(bulletShooterCode%2==1, 8);
            Destroy(gameObject, 3.0f);
            RpcDie();
            Die();
        }
        else RefreshHp();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isServer) return;
        if (!move.activeSelf) return;

        Transform minTransform = null;
        float minDist = float.MaxValue;
        foreach (var playerTransform in playerTransforms)
        {
            float dist = Vector3.Distance(transform.position, playerTransform.position);
            if (dist < minDist && dist < 80f)
            {
                minDist = dist;
                minTransform = playerTransform;
            }
        }
        targetTransform = minTransform;

        if (targetTransform != null)
        {
            if (transform.position.x <= targetTransform.position.x) onFlip = false;
            else onFlip = true;
            ChangeFlipState();
            Vector3 newVec = (targetTransform.position - transform.position).normalized * speed;
            newVec.z = 0.0f;
            _rb.velocity = newVec;
            if(Vector2.Distance(transform.position, targetTransform.position) <= 3.5f)
            {
                RpcAttackStart();
                AttackStart();
            }
        }
    }
    [ClientRpc]
    void RpcAttackStart()
    {
        AttackStart();
    }
    void AttackStart()
    {
        move.SetActive(false);
        attack.GetComponent<Animator>().Rebind();
        attack.SetActive(true);
        Invoke("AttackEnd", attack.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
    void AttackEnd()
    {
        if (onDie) return;
        move.SetActive(true);
        attack.SetActive(false);
        if (!isServer) return;
        GameObject other = targetTransform.gameObject;
        if (Vector2.Distance(other.transform.position, transform.position) <= 4.5f)
        {
            var playerMain = other.GetComponent<PlayerMain>();
            playerMain.CmdDie();
        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            var playerMain = other.GetComponent<PlayerMain>();
            if (playerMain.isLocalPlayer)
                playerMain.CmdDie();
        }
    }*/
}
