using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MyNetworkManager : NetworkManager
{
    [HideInInspector]
    public bool isServer;
    public int count = 1;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, GameObject.FindGameObjectWithTag("Prison").transform.position, Quaternion.identity);
        player.GetComponent<PlayerMain>()._playerId = count++;
        player.GetComponent<PlayerMain>().attackAble = false;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        isServer = true;
        FindObjectOfType<UIManager>().ChangeScreen(UIManager.UIState.LOBBY);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        isServer = false;
        FindObjectOfType<UIManager>().ChangeScreen(UIManager.UIState.LOBBY);
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        FindObjectOfType<UIManager>().ChangeScreen(UIManager.UIState.MAIN_MENU);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        FindObjectOfType<UIManager>().ChangeScreen(UIManager.UIState.MAIN_MENU);
    }
    private void Start()
    {
        FindObjectOfType<UIManager>().ChangeScreen(UIManager.UIState.MAIN_MENU);
    }
}
