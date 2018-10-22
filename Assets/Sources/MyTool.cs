using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MyTool
{
    public static GameObject GetPlayerGameObject(int playerId)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach(var player in players)
        {
            if(player.GetComponent<PlayerMain>().PlayerId == playerId) return player;
        }
        return null;
    }
}