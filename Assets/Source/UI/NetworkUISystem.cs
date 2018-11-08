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
    [SerializeField]
    float gameTime = 50f;

    Dictionary<int, Sprite> sprites;
    private void Start()
    {
        Sprite[] spriteObjs = Resources.LoadAll<Sprite>("UI_ingame");
        sprites = new Dictionary<int, Sprite>();
        for(int i=0; i<spriteObjs.Length-2; ++i)
        {
            sprites[(i+1)] = spriteObjs[i+1];
        }
        um = FindObjectOfType<UIManager>();
        score1Text = um.inGameUI.transform.Find("Score1").GetComponent<UnityEngine.UI.Text>();
        score2Text = um.inGameUI.transform.Find("Score2").GetComponent<UnityEngine.UI.Text>();
        timerText = um.inGameUI.transform.Find("Timer").GetComponent<UnityEngine.UI.Text>();
        UpdateScore();
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
    [SerializeField]
    AudioClip gameEndSound;
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
        GetComponent<AudioSource>().PlayOneShot(gameEndSound);
        WinnerPanel.GetInstance().SetWinner(score1, score2);
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
    [SerializeField]
    UnityEngine.UI.Image i10, i11, i12, i13, i20, i21, i22, i23;
    void UpdateScore()
    {
        {
            int t10 = (score1 % 10000) / 1000;
            int t11 = (score1 % 1000) / 100;
            int t12 = (score1 % 100) / 10;
            int t13 = (score1 % 10);
            i10.enabled = false;
            i11.enabled = false;
            i12.enabled = false;
            i13.enabled = false;
            if (t10 > 0)
            {
                i10.enabled = true;
                i11.enabled = true;
                i12.enabled = true;
                i13.enabled = true;
            }
            else if (t11 > 0)
            {
                i11.enabled = true;
                i12.enabled = true;
                i13.enabled = true;
            }
            else if (t12 > 0)
            {
                i12.enabled = true;
                i13.enabled = true;
            }
            i13.enabled = true;
            i10.sprite = sprites[(t10 + 1)];
            i11.sprite = sprites[(t11 + 1)];
            i12.sprite = sprites[(t12 + 1)];
            i13.sprite = sprites[(t13 + 1)];
        }

        {
            int t20 = (score2 % 10000) / 1000;
            int t21 = (score2 % 1000) / 100;
            int t22 = (score2 % 100) / 10;
            int t23 = (score2 % 10);
            i20.enabled = false;
            i21.enabled = false;
            i22.enabled = false;
            i23.enabled = false;
            if (t20 > 0)
            {
                i20.enabled = true;
                i21.enabled = true;
                i22.enabled = true;
                i23.enabled = true;
            }
            else if (t21 > 0)
            {
                i21.enabled = true;
                i22.enabled = true;
                i23.enabled = true;
            }
            else if (t22 > 0)
            {
                i22.enabled = true;
                i23.enabled = true;
            }
            i23.enabled = true;
            i20.sprite = sprites[(t20 + 11)];
            i21.sprite = sprites[(t21 + 11)];
            i22.sprite = sprites[(t22 + 11)];
            i23.sprite = sprites[(t23 + 11)];
        }

        /*if (score1Text != null) score1Text.text = score1 + "";
        if (score2Text != null) score2Text.text = score2 + "";*/
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
        string timerM = ((int)_timer / 60) + "";
        string timerS = ((int)_timer % 60) + "";
        if (timerM.Length == 1) timerM = "0" + timerM;
        if (timerS.Length == 1) timerS = "0" + timerS;
        timerText.text = timerM + ":"  + timerS;
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
