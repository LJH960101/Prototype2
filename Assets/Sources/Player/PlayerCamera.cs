using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    PlayerMain _localPlayer;
    [SerializeField]
    float clampPosX = 12f;
    public void SetLocalCharacter(PlayerMain pm)
    {
        _localPlayer = pm;
    }
	
	// Update is called once per frame
	void Update () {
        if (_localPlayer == null) return;
        if (_localPlayer.transform.position.y >= 50f) return;
        transform.position = _localPlayer.transform.position;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -clampPosX, clampPosX),
            transform.position.y,
            -10.0f);
	}
}
