using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class Monster : MonoBehaviour, ICharacter
{
    [Header("Monster Stat")]
    public float MaxHealth = 100;
    public float CurHealth;
    public int Atk = 10;
    public float AtkSpeed = 1;
    public int AtkRange = 1;
    public int Exp = 10;
    [Space(10.0f)]
    public bool IsChase;
    public bool IsAttack;
    public bool IsDead;
    public Transform Target;
    public BoxCollider BodyCollision;

    Rigidbody Rigid;
    BoxCollider BoxCollision;
    Material mat;
    NavMeshAgent nav;
    Animator anim;
    void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        BoxCollision = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        CurHealth = MaxHealth;
        Invoke("ChaseStart",1);
    }
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void ChaseStart()
    {
        IsChase = true;
        anim.SetBool("IsWalk", true);
    }

    void Update()
    {
        Detect();
        if (nav.enabled)
        {
            TurnToTarget();
            nav.SetDestination(Target.transform.position);
            nav.isStopped = !IsChase;
        }
        
    }

    void FixedUpdate()
    {
        if(Alive())
        {

        }
            Rigid.velocity = Vector3.zero;
            Rigid.angularVelocity = Vector3.zero;
    }
    void TurnToTarget()
    {
        Vector3 TargetVec = (Target.position - transform.position);
        TargetVec.y = 0;
        TargetVec.Normalize();
        transform.rotation = Quaternion.LookRotation(TargetVec);
    }

    void Detect()
    {
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            AtkRange,
            transform.forward, 0, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !IsAttack)
        {
            Attack();
        }

    }

    void Attack()
    {
        TurnToTarget();
        IsChase = false;
        IsAttack = true;
        anim.speed = AtkSpeed;
        anim.SetBool("IsAttack", true);
    }

    public void OnAttackEnd()
    {
        IsChase = true;
        IsAttack = false;
        anim.SetBool("IsAttack", false);
        anim.speed = 1;
    }

    //¹Ì»ç¿ë
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            if ((player.SkillType == 0))
            {
                //ApplyDamage(player.Atk, player);
            }
        }
    }

    public bool Alive()
    {
        return !IsDead;
    }
    public void ApplyDamage(float Damage ,Player DamageCauser)
    {
        CurHealth -= Damage;
        if(CurHealth < 0) CurHealth = 0;

        if (CurHealth <= 0 && !IsDead)
        {
            IsDead = true;
            IsChase = false;
            nav.enabled = false;
            BodyCollision.enabled = false;

            DamageCauser.IncreaseExp(Exp);
            DamageCauser.RemoveTarget();
            DamageCauser.IncreaseExp(Exp);

            gameObject.layer = 9;
            StopAllCoroutines();

            Vector3 ReactVec = transform.position - DamageCauser.transform.position;
            ReactVec = ReactVec.normalized;
            ReactVec += Vector3.up;
            Rigid.AddForce(ReactVec * 5, ForceMode.Impulse);

            anim.SetTrigger("Die");

            Destroy(gameObject, 3.0f);
        }

    }

    public void OnAtkColliderEnable()
    {
        BodyCollision.enabled = true;
    }

    public void OnAtkColliderDisable()
    {
        BodyCollision.enabled = false;
    }

    public void OnAtkTrigger()
    {

    }
    public void OnDeadEnd()
    {
        //Dead Animation End
    }
}
