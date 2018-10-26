using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConstant : NetworkBehaviour
{
    [SyncVar]
    public float Speed = 20.0f;
    [SyncVar]
    public float ShootDelay = 0.2f;
    [SyncVar]
    public int Damage = 10;

    [SerializeField]
    float _itemTime = 10; // 지속 효과 시간
    public float ItemTime{ get { return _itemTime; } }
    [SerializeField]
    int _itemDamage = 5; // 아이템으로 증가하는 뎀
    public int ItemDamage { get { return _itemDamage; } }
    [SerializeField]
    float _itemSpeed = 7; // 아이템으로 증가하는 속도
    public float ItemSpeed { get { return _itemSpeed; } }
    [SerializeField]
    float _itemAttackSpeed = 0.1f; // 아이템으로 감소하는 딜레이
    public float ItemAttackSpeed { get { return _itemAttackSpeed; } }
    [SerializeField]
    int _itemHp = 50; // 아이템으로 증가하는 체력
    public int ItemHp { get { return _itemHp; } }
    [SerializeField]
    float _shootPower = 5.0f;
    public float ShootPower { get { return _shootPower; } }
    [SerializeField]
    float _jumpPower = 200.0f;
    public float JumpPower { get { return _jumpPower; } }
    [SerializeField]
    GameObject _bullet = null;
    public GameObject Bullet { get { return _bullet; } }
    [SerializeField]
    GameObject _bomb = null;
    public GameObject Bomb { get { return _bomb; } }
    [SerializeField]
    Transform _hpBar;
    public Transform HpBar { get { return _hpBar; } }
    [SerializeField]
    GameObject _ragdoll;
    public GameObject RagDoll { get { return _ragdoll; } }
}
