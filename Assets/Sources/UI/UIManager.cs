using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour {
    public enum UIState
    {
        MAIN_MENU,
        LOBBY,
        INGAME,
        MAX
    };
    UIState currentState;
    public UIState State { get { return currentState; } }
    public GameObject inGameUI, lobbyUi;
    NetworkManagerHUD networkHUD;
    [HideInInspector]
    public NetworkUISystem networkUISystem;

    private void Awake()
    {
        networkHUD = GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>();
        networkUISystem = transform.Find("NetworkUIManager").GetComponent<NetworkUISystem>();
    }

    public void ChangeScreen(UIState targetState)
    {
        GetComponent<InGameUI>().enabled = false;
        GetComponent<LobbyUI>().enabled = false;
        GetComponent<MainMenuUI>().enabled = false;
        networkHUD.showGUI = false;
        inGameUI.SetActive(false);
        lobbyUi.SetActive(false);

        switch (targetState)
        {
            case UIState.MAIN_MENU:
                networkHUD.showGUI = true;
                GetComponent<MainMenuUI>().enabled = true;
                Cursor.visible = true;
                break;
            case UIState.LOBBY:
                GetComponent<LobbyUI>().enabled = true;
                GetComponent<LobbyUI>().Enter();
                lobbyUi.SetActive(true);
                Cursor.visible = true;
                break;
            case UIState.INGAME:
                GetComponent<InGameUI>().enabled = true;
                inGameUI.SetActive(true);
                Cursor.visible = false;
                break;
            default:
                Debug.LogError("Unkown State");
                break;
        }
        currentState = targetState;
    }
    private static UIManager instance;
    public static UIManager GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }
        return instance;
    }
}
