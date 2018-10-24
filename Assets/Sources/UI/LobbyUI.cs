using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour {
    UIManager _umgr;
	// Use this for initialization
	void Start () {
        _umgr = GetComponent<UIManager>();
        if (_umgr == null) Debug.LogError("None exist cp.");
	}
	
    public void Enter()
    {
        if (_umgr == null) Start();
        if (_umgr.networkUISystem.isServer) _umgr.lobbyUi.transform.Find("StartButton").gameObject.SetActive(true);
        else _umgr.lobbyUi.transform.Find("StartButton").gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
        _umgr.lobbyUi.transform.Find("PlayerCount").GetComponent<UnityEngine.UI.Text>().text = GameObject.FindGameObjectsWithTag("Player").Length + "";
	}
}
