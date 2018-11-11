using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinnerPanel : MonoBehaviour {
    [SerializeField]
    Sprite team1, team2;
    Image winner;
    UnityEngine.UI.Text tscore1, tscore2;
    [SerializeField]
    UnityEngine.UI.Image i10, i11, i12, i13, i20, i21, i22, i23;
    Dictionary<int, Sprite> sprites;
    bool onInit = false;
    private void Start()
    {
        if(!onInit) Init();
    }
    void Init()
    {
        onInit = true;
        Sprite[] spriteObjs = Resources.LoadAll<Sprite>("UI_ingame");
        sprites = new Dictionary<int, Sprite>();
        for (int i = 0; i < spriteObjs.Length - 2; ++i)
        {
            sprites[(i + 1)] = spriteObjs[i + 1];
        }
        tscore1 = transform.Find("Score1").GetComponent<UnityEngine.UI.Text>();
        tscore2 = transform.Find("Score2").GetComponent<UnityEngine.UI.Text>();
        winner = transform.Find("Image").GetComponent<Image>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void SetWinner(int score1, int score2)
    {
        if (!onInit) Init();

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
        if (score1 > score2) winner.sprite = team1;
        else winner.sprite = team2;
    }

    private static WinnerPanel instance;
    public static WinnerPanel GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.Find("Canvas").transform.Find("WinnerPanel").GetComponent<WinnerPanel>();
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }
        return instance;
    }
}
