using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    PlayerConstant _pc;
    PlayerNetwork _pn;
    PlayerMain _pm;
    Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _pc = GetComponent<PlayerConstant>();
        _pn = GetComponent<PlayerNetwork>();
        _pm = GetComponent<PlayerMain>();
    }
    void Jump()
    {
        _rb.AddForce(new Vector2(0f, _pc.JumpPower));
    }
    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    [Command]
    void CmdMoveToPortal()
    {
        transform.position = GameObject.FindGameObjectWithTag("Portal").transform.position;
    }
    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * 1f * _pc.Speed;

        _rb.velocity = new Vector2(x, _rb.velocity.y);

        if(transform.position.x <= -45 || transform.position.x >= 45)
        {
            CmdMoveToPortal();
        }
    }
}
