using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour {
    [Command]
    public void CmdAddScore(bool isTeam1, int score)
    {
        NetworkUISystem.GetInstance().AddScore(isTeam1, score);
    }
}
