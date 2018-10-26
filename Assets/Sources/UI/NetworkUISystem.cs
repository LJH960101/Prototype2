using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkUISystem : NetworkBehaviour
{
    UIManager um;
    public UIManager UM { get { return um; } }
    UnityEngine.UI.Text score1Text, score2Text, timerText;
    public PlayerMain localPlayer;
    [SyncVar(hook = "UpdateScores1")]
    int score1;
    [SyncVar(hook = "UpdateScores2")]
    int score2;
    [SyncVar(hook = "UpdateTimer")]
    float _timer;
    float gameTime = 50f;

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
            if (_timer <= 0.0f && um.State == UIManager.UIState.INGAME)
            {
                _timer = 0.0f;
                GameEnd();
            }
        }
    }
    void GameEnd()
    {
        um.ChangeScreen(UIManager.UIState.LOBBY);
        FindObjectOfType<Spawner>().onSpawn = false;
        RpcOnBackToLobby();
        ShowWinnerPanel();
        _timer = 0f;
        var players = FindObjectsOfType<PlayerMain>();
        foreach (var player in players)
        {
            player.CmdGoToPrison();
        }
        var bullets = FindObjectsOfType<BulletMain>();
        foreach (var bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }
        var monsters = FindObjectsOfType<Monster>();
        foreach (var monster in monsters)
        {
            Destroy(monster.gameObject);
        }
        var moneys = FindObjectsOfType<Money>();
        foreach (var money in moneys)
        {
            Destroy(money.gameObject);
        }
    }
    [ClientRpc]
    public void RpcOnStartButton()
    {
        um.ChangeScreen(UIManager.UIState.INGAME);
    }
    [ClientRpc]
    public void RpcOnBackToLobby()
    {
        ShowWinnerPanel();
        um.ChangeScreen(UIManager.UIState.LOBBY);
    }
    void ShowWinnerPanel()
    {
        if (score1 > score2) WinnerPanel.GetInstance().SetWinner(1);
        else if (score1 < score2) WinnerPanel.GetInstance().SetWinner(2);
        else WinnerPanel.GetInstance().SetWinner(0);
        WinnerPanel.GetInstance().Show();
    }
    public void OnStartButton()
    {
        um.ChangeScreen(UIManager.UIState.INGAME);
        FindObjectOfType<Spawner>().onSpawn = true;
        RpcOnStartButton();
        _timer = gameTime;
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
        timerText.text = (int)_timer + "";
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
