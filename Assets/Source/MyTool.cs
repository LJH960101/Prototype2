using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MyTool
{
    public static GameObject GetPlayerGameObject(int playerId)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponent<PlayerMain>().PlayerId == playerId) return player;
        }
        return null;
    }
    public static PlayerMain GetLocalPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponent<PlayerMain>().isLocalPlayer) return player.GetComponent<PlayerMain>();
        }
        return null;
    }

    static Stuff GetStuffObject(int stuffNumb, GameObject[] objects)
    {
        foreach (var stuffObject in objects)
            if (stuffObject.transform.name == "Stuff" + stuffNumb) return stuffObject.GetComponent<Stuff>();
        return null;
    }
    public static Stuff GetStuff(Stuff.StuffType type)
    {
        var objects = GameObject.FindGameObjectsWithTag("Stuff");
        switch (type)
        {
            case Stuff.StuffType.POWER:
                return GetStuffObject(1, objects);
            case Stuff.StuffType.HP:
                return GetStuffObject(2, objects);
            case Stuff.StuffType.BOMB:
                return GetStuffObject(3, objects);
            case Stuff.StuffType.SPEED:
                return GetStuffObject(4, objects);
            case Stuff.StuffType.ATTACK_SPEED:
                return GetStuffObject(5, objects);
            default:
                Debug.LogError("Wrong stuff code.");
                return null;
        }
    }
    public static Stuff GetStuff(int stuffType)
    {
        var objects = GameObject.FindGameObjectsWithTag("Stuff");
        return GetStuffObject(stuffType, objects);
    }
    public static void Log(string log)
    {
        UnityEngine.UI.Text text = GameObject.Find("Canvas").transform.Find("Log").GetComponent<UnityEngine.UI.Text>();
        text.text = text.text + log + "\n";
    }
}