using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour {
    public enum StuffType {
        POWER,
        HP,
        BOMB,
        SPEED,
        ATTACK_SPEED,
        MAX
    }
    public StuffType stuffType;

    [SerializeField]
    UnityEngine.UI.Text nameText, priceText;
    [SerializeField]
    UnityEngine.UI.Image panel, timerPanel;
    float coolTime, coolTimeTimer;
    float originCoolTime;
    int upgradeCount = 0;

    private void Start()
    {
        // 쿨타임 초기화
        switch (stuffType)
        {
            case StuffType.BOMB:
                coolTime = 15.0f;
                break;
            case StuffType.SPEED:
                coolTime = 10.5f;
                break;
            case StuffType.ATTACK_SPEED:
                coolTime = 10.5f;
                break;
            default:
                coolTime = 0.0f;
                break;
        }
        originCoolTime = coolTime;
        coolTimeTimer = 0.0f;
        panel.gameObject.SetActive(false);
        timerPanel.fillAmount = 1.0f;
        timerPanel.gameObject.SetActive(false);
    }
    public int GetPrice()
    {
        switch (stuffType)
        {
            case StuffType.POWER:
            case StuffType.HP:
                if (upgradeCount == 0) return 50;
                else if (upgradeCount == 1) return 100;
                else if (upgradeCount == 2) return 150;
                else if (upgradeCount == 3) return 200;
                else if (upgradeCount == 4) return 300;
                else return int.MaxValue;
            case StuffType.BOMB:
                return 300;
            case StuffType.SPEED:
                return 500;
            case StuffType.ATTACK_SPEED:
                return 500;
        }
        return int.MaxValue;
    }
    public bool Use()
    {
        if (coolTimeTimer > 0.001f) return false;
        int beforePrice = GetPrice();
        switch (stuffType)
        {
            case StuffType.POWER:
                {
                    if (upgradeCount >= 5) return false;
                    string newText = "공격력 강화";
                    upgradeCount += 1;
                    if (upgradeCount >= 1) newText = newText + " +" + upgradeCount;
                    nameText.text = newText;
                    priceText.text = GetPrice() + " PC";
                    if (upgradeCount >= 5)
                    {
                        priceText.text = "";
                        panel.gameObject.SetActive(true);
                    }
                }
                break;
            case StuffType.HP:
                {
                    if (upgradeCount >= 5) return false;
                    string newText = "방어력 강화";
                    upgradeCount += 1;
                    if (upgradeCount >= 1) newText = newText + " +" + upgradeCount;
                    nameText.text = newText;
                    priceText.text = GetPrice() + " PC";
                    if (upgradeCount >= 5)
                    {
                        priceText.text = "";
                        panel.gameObject.SetActive(true);
                    }
                }
                break;
            case StuffType.BOMB:
                StartCoolTime();
                break;
            case StuffType.SPEED:
                StartCoolTime();
                break;
            case StuffType.ATTACK_SPEED:
                StartCoolTime();
                break;
        }
        var effect = Instantiate(stuffEffect, GameObject.Find("Canvas").transform);
        effect.GetComponent<UnityEngine.UI.Image>().sprite = transform.Find("IM_Image").GetComponent<UnityEngine.UI.Image>().sprite;
        effect.transform.Find("TX_Price").GetComponent<UnityEngine.UI.Text>().text = beforePrice + " PC";
        MyTool.GetLocalPlayer().GetComponent<AudioSource>().PlayOneShot(shopSound);
        StartAllShortCoolTime();
        return true;
    }
    [SerializeField]
    AudioClip shopSound;
    [SerializeField]
    GameObject stuffEffect;
    void StartAllShortCoolTime()
    {
        var stuffs = GameObject.FindObjectsOfType<Stuff>();
        foreach(Stuff stuff in stuffs)
        {
            if (stuff.coolTimeTimer <= 0f) stuff.StartShortCoolTime();
        }
    }
    public void StartShortCoolTime()
    {
        panel.gameObject.SetActive(true);
        timerPanel.fillAmount = 1.0f;
        timerPanel.gameObject.SetActive(true);
        coolTimeTimer = 1f;
        coolTime = 1f;
        Invoke("BackToOriginCoolTime", 1f);
    }
    void BackToOriginCoolTime()
    {
        coolTime = originCoolTime;
    }
    void StartCoolTime()
    {
        panel.gameObject.SetActive(true);
        timerPanel.fillAmount = 1.0f;
        timerPanel.gameObject.SetActive(true);
        coolTimeTimer = coolTime;
    }
    private void Update()
    {
        if (coolTimeTimer > 0.0f)
        {
            coolTimeTimer -= Time.deltaTime;
            timerPanel.fillAmount = coolTimeTimer / coolTime;
            if(coolTimeTimer <= 0.0f)
            {
                panel.gameObject.SetActive(false);
                timerPanel.fillAmount = 1.0f;
                timerPanel.gameObject.SetActive(false);
            }
        }
    }
}
