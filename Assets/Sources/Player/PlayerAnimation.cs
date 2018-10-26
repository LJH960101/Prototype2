using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    Animator _animator;
    Rigidbody _rb;
    PlayerConstant _pc;
    Transform _model;
    bool onJump;
    public bool OnJump { get { return onJump; } }
    float distToGround;
    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _model = transform;
        _rb = transform.parent.GetComponent<Rigidbody>();
        _pc = transform.parent.GetComponent<PlayerConstant>();
        onJump = false;
        distToGround = transform.parent.GetComponent<Collider>().bounds.extents.y;
    }
	public void RunShootAnimation()
    {
        _animator.Play("Attack", 1);
        _animator.SetBool("OnAttack", true);
    }
    public void AttackEnd()
    {
        _animator.SetBool("OnAttack", false);
    }
	// Update is called once per frame
	void Update ()
    {
        // 대가리 돌리기
        if (_rb.velocity.x > 0)
            _model.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        else if (_rb.velocity.x < 0)
            _model.transform.eulerAngles = new Vector3(0f, -90f, 0f);

        // Y축 변동이 있으면 점프 중임
        if (Mathf.Abs(_rb.velocity.y) > 0.001f) onJump = true;
        // 점프 중일때 닿았는지 처리하기
        else if(onJump && IsGrounded()) onJump = false;

        _animator.SetFloat("velocityX", Mathf.Abs(_rb.velocity.x / _pc.Speed));
        _animator.SetBool("OnJump", onJump);
    }

    bool IsGrounded(){
        return Physics.Raycast(transform.parent.position, -Vector3.up, distToGround + 0.1f);
    }
}
