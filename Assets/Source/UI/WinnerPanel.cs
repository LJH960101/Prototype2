using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerPanel : MonoBehaviour {
    UnityEngine.UI.Text text;
    private void Start()
    {
        text = transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
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
        if(text == null) text = transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
        string newText = "1팀: " + score1 + "\n2팀: " + score2 + "\n최종 승리: ";
        if (score1 > score2) newText = newText + "1팀";
        else newText = newText + "2팀";
        text.text = newText;
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
