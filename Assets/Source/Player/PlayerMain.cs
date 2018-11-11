
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMain : NetworkBehaviour {
    PlayerConstant _pc;
    PlayerNetwork pn;
    GameObject _myAim = null;
    UnityEngine.UI.Text _moneyText;
    AudioSource _as;
    [SerializeField]
    AudioClip dieSound;
    bool hitable = true;
    [SyncVar]
    public int _playerId = -1;
    [SyncVar(hook ="OnMoneyChange")]
    int _money = 0;
    [SyncVar(hook = "OnMaxHpChange")]
    public int _maxHp = 100;
    [SyncVar(hook = "OnHpChange")]
    int _hp = 100;
    [SerializeField]
    Material team1, team2;
    public bool attackAble = true;
    public int Money { get { return _money; } set { _money = value; } }
    public int PlayerId { get { return _playerId; } }
    public override void OnStartLocalPlayer()
    {
        _myAim = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerAim"));
    }
    void OnMoneyChange(int money)
    {
        if (!isServer) _money = money;
        ShowMoney();
    }
    void RefreshHp()
    {
        _pc.HpBar.localScale = new Vector3((float)_hp / (float)_maxHp, 1.0f, 1.0f);
        _pc.HpBar.localPosition = new Vector3( - (1.0f - ((float)_hp / (float)_maxHp)), 0.0f, 0.0f);
        float scale = 0.8f + (float)_maxHp / 400f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    void OnMaxHpChange(int maxHp)
    {
        if (!isServer) _maxHp = maxHp;
        RefreshHp();
    }
    void OnHpChange(int hp)
    {
        if (!isServer) _hp = hp;
        RefreshHp();
    }
    void ShowMoney()
    {
        if (isLocalPlayer)
            _moneyText.text = _money + " PC";
    }
    private void OnDestroy()
    {
        if (_myAim != null)
        {
            Destroy(_myAim);
            _myAim = null;
        }
    }

    List<Monster> monsters;
    Dictionary<Monster, GameObject> monsterFinders;
    [SerializeField]
    GameObject monsterFinderObject;
    
    public void AddMonster(Monster monster)
    {
        monsters.Add(monster);
        monsterFinders[monster] = Instantiate(monsterFinderObject, GameObject.Find("Canvas").transform);
    }
    public void RemoveMonster(Monster monster)
    {
        monsters.Add(monster);
        if (monsterFinders.ContainsKey(monster))
        {
            Destroy(monsterFinders[monster]);
            monsterFinders.Remove(monster);
        }
        else
        {
            monsterFinders.Clear();
        }
    }

    public void CalcPos(Monster monster)
    {
        CalcPos(monster, monsterFinders[monster]);
    }
    public void CalcPos(Monster monster, GameObject finder)
    {
        Vector3 moveDirection = monster.transform.position - myCamera.transform.position;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        finder.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        Vector2 newVec = monster.transform.position - myCamera.transform.position;
        newVec.x = Mathf.Clamp(newVec.x * 30f, -860f, 860f);
        newVec.y = Mathf.Clamp(newVec.y * 30f, -480f, 430f);

        if (newVec.x >= -850f && newVec.x <= 850f && newVec.y >= -470f && newVec.y <= 420f) finder.SetActive(false);
        else finder.SetActive(true);
        finder.GetComponent<RectTransform>().anchoredPosition = newVec;
    }

    void SyncHealthToClient()
    {
        RpcSyncHealth(_hp);
    }
    [ClientRpc]
    void RpcSyncHealth(int health)
    {
        _hp = health;
        RefreshHp();
    }
    float syncTimer = 1.0f;
    private void Update()
    {
        if (isServer)
        {
            syncTimer -= Time.deltaTime;
            if(syncTimer <= 0f)
            {
                SyncHealthToClient();
                syncTimer += 1.0f;
            }
        }
        if (!attackAble && !isLocalPlayer) return;
        foreach(var monsterFinder in monsterFinders)
        {
            Monster monster = monsterFinder.Key;
            GameObject finder = monsterFinder.Value;

            CalcPos(monster, finder);
        }
    }

    [Command]
    public void CmdAddMoney(int money)
    {
        _money += money;
        ShowMoney();
    }

    [Command]
    public void CmdAddMaxHp()
    {
        _maxHp += _pc.ItemHp;
        _hp += _pc.ItemHp;
        RefreshHp();
    }

    public void GetDamage(int damage)
    {
        if (!isServer) return;
        if (!hitable) return;
        if (isServer) syncTimer += 1.0f;
        _hp -= damage;
        if (_hp <= 0 && isServer) CmdDie();
        else RefreshHp();
    }

    [Command]
    public void CmdDie()
    {
        if (!hitable) return;
        hitable = false;
        bool isTeam1 = PlayerId % 2 == 0;
        pn.CmdAddScore(isTeam1, 1);
        _hp = _maxHp;
        RefreshHp();
        RpcSpawnRagdoll();
        SpawnRagdoll();
        CmdGoToPrison();
        Invoke("Spawn", 3.0f);
    }
    [Command]
    public void CmdGoToPrison()
    {
        transform.Find("Model").gameObject.SetActive(false);
        transform.Find("HPFrame").gameObject.SetActive(false);
        attackAble = false;
        CmdMoveTo(GameObject.FindGameObjectWithTag("Prison").transform.position);
    }
    [ClientRpc]
    public void RpcSpawnRagdoll()
    {
        SpawnRagdoll();
    }
    [SerializeField]
    GameObject effect1, effect2;
    void SpawnRagdoll()
    {
        _as.PlayOneShot(dieSound);
        GameObject ragdoll = Instantiate(_pc.RagDoll, transform.position, transform.rotation);
        CopyTransform(transform.Find("Model"), ragdoll.transform);
        ragdoll.transform.localScale = transform.localScale;
        if (PlayerId % 2 == 0) Instantiate(effect1, transform.position, Quaternion.Euler(new Vector3(-90, 0f, 0f)));
        else Instantiate(effect2, transform.position, Quaternion.Euler(new Vector3(-90, 0f, 0f)));

        if (_playerId % 2 == 1)
            ragdoll.transform.Find("Cylinder001").GetComponent<Renderer>().material = team1;
        else
            ragdoll.transform.Find("Cylinder001").GetComponent<Renderer>().material = team2;

        Destroy(ragdoll, 3.0f);
    }

    void CopyTransform(Transform source, Transform target)
    {
        target.position = source.position;
        target.rotation = source.rotation;
        target.localScale = source.localScale;
        if (source.childCount == 0) return;
        else
        {
            for (int i = 0; i < source.childCount; ++i)
            {
                CopyTransform(source.GetChild(i), target.GetChild(i));
            }
        }
    }

    [Command]
    public void CmdMoveTo(Vector3 newPosition)
    { //call this on the server
        transform.position = newPosition; //so the player moves also in the server
        RpcMoveTo(transform.position);
    }
    [ClientRpc]
    void RpcMoveTo(Vector3 newPosition)
    {
        transform.position = newPosition; //this will run in all clients
    }

    public void Spawn()
    {
        if (NetworkUISystem.GetInstance().UM.State != UIManager.UIState.INGAME) return;
        transform.Find("Model").gameObject.SetActive(true);
        transform.Find("HPFrame").gameObject.SetActive(true);
        attackAble = true;
        CmdMoveTo(GameObject.FindGameObjectWithTag("Spawn" + (((PlayerId + 1) % 2) + 1)).transform.position);
        hitable = false;
        Invoke("SetHittable", 2f);
    }

    void SetHittable()
    {
        hitable = true;
    }

    GameObject myCamera;
    void RefreshMaterial()
    {
        if (_playerId % 2 == 1)
            transform.Find("Model").Find("Cylinder001").GetComponent<Renderer>().material = team1;
        else
            transform.Find("Model").Find("Cylinder001").GetComponent<Renderer>().material = team2;
    }
    // Use this for initialization
    void Start ()
    {
        Invoke("RefreshMaterial", 2f);
        _as = GetComponent<AudioSource>();
        monsters = new List<Monster>();
        monsterFinders = new Dictionary<Monster, GameObject>();
        _pc = GetComponent<PlayerConstant>();
        pn = GetComponent<PlayerNetwork>();
        if (isLocalPlayer)
        {
            FindObjectOfType<NetworkUISystem>().localPlayer = this;
            _moneyText = UIManager.GetInstance().inGameUI.transform.Find("Money").GetComponent<UnityEngine.UI.Text>();
            if (_moneyText == null) Debug.LogError("Cant get money text");
            FindObjectOfType<PlayerCamera>().SetLocalCharacter(this);
            myCamera = FindObjectOfType<PlayerCamera>().gameObject;
        }
    }
}
