using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMain : NetworkBehaviour {
    GameObject _myAim = null;
    UnityEngine.UI.Text _moneyText;
    [SyncVar]
    int _playerId = -1;
    [SyncVar(hook ="OnMoneyChange")]
    int _money = 0;
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

    // Use this for initialization
    void Start ()
    {
        if (isServer)
        {
            _playerId = GameObject.FindGameObjectsWithTag("Player").Length;
        }
        if (isLocalPlayer)
        {
            FindObjectOfType<NetworkUISystem>().localPlayer = this;
            _moneyText = UIManager.GetInstance().inGameUI.transform.Find("Money").GetComponent<UnityEngine.UI.Text>();
            if (_moneyText == null) Debug.LogError("Cant get money text");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
