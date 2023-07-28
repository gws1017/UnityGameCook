using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class Monster : MonoBehaviour
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
        if(IsChase)
        {
            Rigid.velocity = Vector3.zero;
            Rigid.angularVelocity = Vector3.zero;
        }
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
            StartCoroutine(Attack());
        }

    }

    IEnumerator Attack()
    {
        TurnToTarget();
        IsChase = false;
        IsAttack = true;
        anim.SetBool("IsAttack", true);
        anim.speed = AtkSpeed;
        yield return new WaitForSeconds(0.2f* AtkSpeed);
        BodyCollision.enabled = true;
        yield return new WaitForSeconds(1f* AtkSpeed);
        BodyCollision.enabled = false;

        IsChase = true;
        anim.speed = 1;
        IsAttack = false;
        anim.SetBool("IsAttack", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;
            Debug.Log("Monster be attacked ");

            if ((player.SkillType == 0))
            {
                CurHealth -= player.Atk;
                if(CurHealth < 0) CurHealth = 0;
                Vector3 ReactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(ReactVec,player));
            }
        }
    }
    public void ApplyDamage(float Damage)
    {
        CurHealth -= Damage;
        if(CurHealth < 0) CurHealth=0;

    }
    IEnumerator OnDamage(Vector3 ReactVec,Player player)
    {
        yield return new WaitForSeconds(0.1f);

        if(CurHealth <= 0 )
        {
            if (IsChase) player.IncreaseExp(Exp);
            player.RemoveTarget();
            player.IncreaseExp(Exp);
            gameObject.layer = 9;
            StopAllCoroutines();

            IsChase = false;
            nav.enabled = false;
            BodyCollision.enabled = false;
            anim.SetTrigger("Die");

            ReactVec = ReactVec.normalized;
            ReactVec += Vector3.up;
            Rigid.AddForce(ReactVec * 5, ForceMode.Impulse);
            
            Destroy(gameObject, 3.0f);
        }
    }

    public void HitFromAtk2(Player DamageCauser)
    {
        CurHealth -= DamageCauser.Atk;
        Vector3 ReactVec = transform.position - DamageCauser.transform.position;
        StartCoroutine(OnDamage(ReactVec, DamageCauser));

    }
}
