using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    PlayerMain _localPlayer;
    PlayerAim _pa;
    [SerializeField]
    float clampPosX = 12f;
    [SerializeField]
    float clampPosY = 20f;
    [SerializeField]
    float cameraSpeed = 3f;
    Vector3 targetVec;
    public void SetLocalCharacter(PlayerMain pm)
    {
        _localPlayer = pm;
    }
	public void SetPlayerAim(PlayerAim pa)
    {
        _pa = pa;
    }

	// Update is called once per frame
	void Update () {
        if (_localPlayer == null || _pa == null) return;
        if (_localPlayer.transform.position.y >= 50f) return;
        //targetVec = (_localPlayer.transform.position * 3 + _pa.transform.position) / 4;
        targetVec = _localPlayer.transform.position;
        targetVec = new Vector3(Mathf.Clamp(targetVec.x, -clampPosX, clampPosX),
            Mathf.Clamp(targetVec.y, -clampPosY, clampPosY),
            -10.0f);

        transform.position = Vector3.MoveTowards(transform.position, targetVec, cameraSpeed);
	}
}
