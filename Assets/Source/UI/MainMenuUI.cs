using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;

public class MainMenuUI : MonoBehaviour
{
    /*
    UIManager _umgr;
    NetworkManager _nmgr;
    private void Start()
    {
        _umgr = GetComponent<UIManager>();
        _nmgr = GetComponent<NetworkManager>();
        if (_umgr == null) Debug.LogError("Not Exist component : UIManager");
        _nmgr.StartMatchMaker();
    }
    void SetPort()
    {
        _nmgr.networkPort = 7777;
    }
    void SetIP()
    {
        _nmgr.networkAddress = _umgr.mainMenuUi.transform.Find("IF_IpAdress").GetComponent<TMPro.TMP_InputField>().text;
    }
    string GetMatchName()
    {
        return _umgr.mainMenuUi.transform.Find("IF_RoomName").GetComponent<TMPro.TMP_InputField>().text;
    }
    public void StartUpLanHost()
    {
        SetPort();
        _nmgr.StartHost();
        _umgr.ChangeScreen(UIManager.UIState.LOBBY);
    }
    public void JoinLanGame()
    {
        SetIP();
        SetPort();
        _nmgr.StartClient();
        _umgr.ChangeScreen(UIManager.UIState.LOBBY);
    }
    public void StartUpInetGame()
    {
        _nmgr.matchMaker.CreateMatch(GetMatchName(), 8, true, "", "", "", 0, 0, OnInternetMatchCreate);
    }
    //this method is called when your request for creating a match is returned
    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 7777);
            _nmgr.StartHost(hostInfo);
            _umgr.ChangeScreen(UIManager.UIState.LOBBY);
        }
        else
        {
            Debug.LogError("Create match failed");
        }
    }
    public void JoinInetGame()
    {
        _nmgr.matchMaker.ListMatches(0, 10, GetMatchName(), true, 0, 0, OnInternetMatchList);
    }
    //this method is called when a list of matches is returned
    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            if (matches.Count != 0)
            {
                _nmgr.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
            }
            else
            {
                Debug.Log("No matches in requested room!");
            }
        }
        else
        {
        }
    }
    //this method is called when your request to join a match is returned
    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            //Debug.Log("Able to join a match");

            MatchInfo hostInfo = matchInfo;
            _nmgr.StartClient(hostInfo);
            _umgr.ChangeScreen(UIManager.UIState.LOBBY);
        }
        else
        {
            Debug.LogError("Join match failed");
        }
    }*/
}
