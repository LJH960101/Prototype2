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
    public void SetWinner(int whoIsWinner)
    {
        if(text == null) text = transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
        if (whoIsWinner == 1) text.text = "팀 1이 이겼습니다!!";
        else if (whoIsWinner == 2) text.text = "팀 2가 이겼습니다!!";
        else text.text = "자강두천 오지네";
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
