using System.Collections;
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

    void OnFlip(bool flip)
    {
        if (!isServer) onFlip = flip;
        ChangeFlipState();
    }
    void ChangeFlipState()
    {
        _sr.flipX = onFlip;
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
        hpBar.localPosition = new Vector3(-(1.0f - ((float)_hp / (float)_maxHp)), 0.0f, -1.0f);
    }

    List<Transform> playerTransforms;
    Transform targetTransform = null;
	// Use this for initialization
	void Start () {
        playerTransforms = new List<Transform>();
        var players = GameObject.FindObjectsOfType<PlayerMain>();
        foreach (var player in players) playerTransforms.Add(player.transform);
        _hp = _maxHp;
        RefreshHp();
    }

    public void GetDamage(int damage, int bulletShooterCode)
    {
        if (!isServer) Debug.Log("GetDamage Only Work on server.... It will be ignore..");
        _hp -= damage;
        if (_hp <= 0)
        {
            MyTool.GetPlayerGameObject(bulletShooterCode).GetComponent<PlayerNetwork>().CmdAddScore(bulletShooterCode%2==1, 800);
            Destroy(gameObject);
        }
        else RefreshHp();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isServer) return;

        if (targetTransform != null)
        {
            if (transform.position.x <= targetTransform.position.x) onFlip = false;
            else onFlip = true;
            ChangeFlipState();
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed);
        }

        Transform minTransform = null;
        float minDist = float.MaxValue;
        foreach (var playerTransform in playerTransforms)
        {
            float dist = Vector3.Distance(transform.position, playerTransform.position);
            if (dist < minDist && dist < 40f)
            {
                minDist = dist;
                minTransform = playerTransform;
            }
        }
        targetTransform = minTransform;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if(other.transform.tag == "Player")
        {
            other.GetComponent<PlayerMain>().CmdDie();
        }
    }
}
