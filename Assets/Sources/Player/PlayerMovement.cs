using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    PlayerConstant _pc;
    PlayerNetwork _pn;
    Rigidbody2D _rb2d;
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _pc = GetComponent<PlayerConstant>();
        _pn = GetComponent<PlayerNetwork>();
    }
    void Jump()
    {
        _rb2d.AddForce(new Vector2(0f, _pc.JumpPower));
    }
    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.F1)) _pn.CmdAddScore(true, 1);
        if (Input.GetKeyDown(KeyCode.F2)) _pn.CmdAddScore(false, 1);
        if (Input.GetKeyDown(KeyCode.F3)) GetComponent<PlayerMain>().CmdAddMoney(10);
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * 1f * _pc.Speed;

        _rb2d.velocity = new Vector2(x, _rb2d.velocity.y);
    }
}
