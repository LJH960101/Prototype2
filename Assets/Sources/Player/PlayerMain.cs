
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMain : NetworkBehaviour {
    PlayerConstant _pc;
    PlayerNetwork pn;
    GameObject _myAim = null;
    UnityEngine.UI.Text _moneyText;
    bool hitable = true;
    [SyncVar]
    public int _playerId = -1;
    [SyncVar(hook ="OnMoneyChange")]
    int _money = 0;
    [SyncVar(hook = "OnMaxHpChange")]
    public int _maxHp = 100;
    [SyncVar(hook = "OnHpChange")]
    int _hp = 100;
    [SyncVar]
    public bool attackAble = true;
    public int Money { get { return _money; } }
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
            _moneyText.text = _money + "";
    }
    private void OnDestroy()
    {
        if (_myAim != null)
        {
            Destroy(_myAim);
            _myAim = null;
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

    [Command]
    public void CmdGetDamage(int damage)
    {
        if (!hitable) return;
        _hp -= damage;
        if (_hp <= 0) CmdDie();
        else RefreshHp();
    }

    [Command]
    public void CmdDie()
    {
        if (!hitable) return;
        hitable = false;
        bool isTeam1 = PlayerId % 2 == 0;
        pn.CmdAddScore(isTeam1, 100);
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
        attackAble = false;
        CmdMoveTo(GameObject.FindGameObjectWithTag("Prison").transform.position);
    }
    [ClientRpc]
    public void RpcSpawnRagdoll()
    {
        SpawnRagdoll();
    }
    void SpawnRagdoll()
    {
        GameObject ragdoll = Instantiate(_pc.RagDoll, transform.position, transform.rotation);
        CopyTransform(transform.Find("Model"), ragdoll.transform);
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
        attackAble = true;
        CmdMoveTo(GameObject.FindGameObjectWithTag("Spawn" + (((PlayerId + 1) % 2) + 1)).transform.position);
        hitable = false;
        Invoke("SetHittable", 2f);
    }

    void SetHittable()
    {
        hitable = true;
    }

    // Use this for initialization
    void Start ()
    {
        _pc = GetComponent<PlayerConstant>();
        pn = GetComponent<PlayerNetwork>();
        if (isLocalPlayer)
        {
            FindObjectOfType<NetworkUISystem>().localPlayer = this;
            _moneyText = UIManager.GetInstance().inGameUI.transform.Find("Money").GetComponent<UnityEngine.UI.Text>();
            if (_moneyText == null) Debug.LogError("Cant get money text");
            FindObjectOfType<PlayerCamera>().SetLocalCharacter(this);
        }
    }
}
