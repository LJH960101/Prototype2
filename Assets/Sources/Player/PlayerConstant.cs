using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConstant : MonoBehaviour
{
    [SerializeField]
    float _shootPower = 5.0f;
    public float ShootPower { get { return _shootPower; } }
    [SerializeField]
    float _speed = 5.0f;
    public float Speed { get { return _speed; } }
    [SerializeField]
    float _jumpPower = 200.0f;
    public float JumpPower { get { return _jumpPower; } }
    [SerializeField]
    GameObject _bullet = null;
    public GameObject Bullet { get { return _bullet; } }
    [SerializeField]
    Transform _hpBar;
    public Transform HpBar { get { return _hpBar; } }
    [SerializeField]
    GameObject _ragdoll;
    public GameObject RagDoll { get { return _ragdoll; } }
}
