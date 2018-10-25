using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkUISystem : NetworkBehaviour
{
    UIManager um;
    UnityEngine.UI.Text score1Text, score2Text, timerText;
    public PlayerMain localPlayer;
    [SyncVar(hook = "UpdateScores1")]
    int score1;
    [SyncVar(hook = "UpdateScores2")]
    int score2;
    [SyncVar(hook = "UpdateTimer")]
    float _timer;

    private void Start()
    {
        um = FindObjectOfType<UIManager>();
        score1Text = um.inGameUI.transform.Find("Score1").GetComponent<UnityEngine.UI.Text>();
        score2Text = um.inGameUI.transform.Find("Score2").GetComponent<UnityEngine.UI.Text>();
        timerText = um.inGameUI.transform.Find("Timer").GetComponent<UnityEngine.UI.Text>();
    }
    private void Update()
    {
        if (isServer)
        {
            _timer -= Time.deltaTime;
            UpdateTimerUI();
        }
    }
    [ClientRpc]
    public void RpcOnStartButton()
    {
        um.ChangeScreen(UIManager.UIState.INGAME);
    }
    public void OnStartButton()
    {
        um.ChangeScreen(UIManager.UIState.INGAME);
        FindObjectOfType<Spawner>().onSpawn = true;
        RpcOnStartButton();
        _timer = 60f;
        var players = FindObjectsOfType<PlayerMain>();
        foreach(var player in players)
        {
            player.Spawn();
        }
    }
    void UpdateScores1(int score)
    {
        if(!isServer) score1 = score;
        UpdateScore();
    }
    void UpdateScores2(int score)
    {
        if (!isServer) score2 = score;
        UpdateScore();
    }
    void UpdateScore()
    {
        if (score1Text != null) score1Text.text = score1 + "";
        if (score2Text != null) score2Text.text = score2 + "";
    }
    public void AddScore(bool isTeam1, int score)
    {
        if (!isServer) return;
        if (isTeam1) score1 += score;
        else score2 += score;
        UpdateScore();
    }
    void UpdateTimer(float timer)
    {
        if (!isServer) _timer = timer;
        UpdateTimerUI();
    }
    void UpdateTimerUI()
    {
        timerText.text = _timer + "";
    }
    private static NetworkUISystem instance;
    public static NetworkUISystem GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindGameObjectWithTag("NetworkUIManager").GetComponent<NetworkUISystem>();
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }
        return instance;
    }
}
