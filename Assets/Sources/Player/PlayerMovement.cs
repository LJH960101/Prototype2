using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    PlayerConstant _pc;
    Rigidbody2D _rb2d;
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _pc = GetComponent<PlayerConstant>();
    }
    void Jump()
    {
        _rb2d.AddForce(new Vector2(0f, _pc.JumpPower));
    }
    // Update is called once per frame
    void Update ()
    {
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * 1f * _pc.Speed;
        if (Input.GetKeyDown(KeyCode.W)) Jump();

        _rb2d.velocity = new Vector2(x, _rb2d.velocity.y);
    }
}
