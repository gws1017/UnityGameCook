using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Player : MonoBehaviour
{
    [Header("Player Stat")]
    public int Level = 1;
    public float MaxExp = 100;
    public float Exp = 0;
    [Space(10.0f)]
    public float MaxHealth = 100;
    public float CurHealth = 100;
    public int Atk = 10;
    public float AtkSpeed = 1;
    public int AtkRange = 1;
    public float Speed = 10.0f;

    [Space(10.0f)]
    public int SkillType = 0;
    public int MaxSkillType = 3;
    public int Skill2Area = 2;

    
    public bool IsDead = false;
    float AtkDelay;
    float Dist;
    Vector3 TargetVec;

    Rigidbody Rigid;
    Animator Anim;
    MeshRenderer[] meshs;

    public List<GameObject> Monsters;
    public GameObject Target;

    void Awake()
    {
        CurHealth = MaxHealth;
        Anim = GetComponentInChildren<Animator>();
        Rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        Detect();
        Move();

        Attack();
    }
    void FixedUpdate()
    {
        Rigid.velocity = Vector3.zero;
        Rigid.angularVelocity = Vector3.zero;
    }

    void Detect()
    {
        if (Monsters.Any() && Target == null)
        {
            Target = Monsters.Last();
            Dist = (Target.transform.position - transform.position).sqrMagnitude;
            for(int i = Monsters.Count - 1; i >= 0; i--) 
            {
                if (Monsters[i] == null) continue;
                float d = (Monsters[i].transform.position - transform.position).sqrMagnitude;
                if (Dist > d)
                {
                    Dist = d;
                    Target = Monsters[i];
                }
            }
        }
        if (Target)
            Dist = (Target.transform.position - transform.position).sqrMagnitude;

    }
    public void IncreaseExp(int exp)
    {
        Exp += exp;
        if(Exp>=MaxExp)
        {
            Exp = Exp - MaxExp;
            Level++;
            CurHealth = MaxHealth;
        }
    }
    public bool Alive()
    {
        return !IsDead;
    }
    void Die()
    {
        if (IsDead == true) return;
        Anim.speed = 1.0f;
        Anim.SetTrigger("Die");
        IsDead = true;
    }

    public void OnDeadEnd()
    {
        IsDead = false;
        CurHealth = MaxHealth;
    }
    void TurnToTarget()
    {
        TargetVec = (Target.transform.position - transform.position);
        TargetVec.y = 0;
        TargetVec.Normalize();
        transform.rotation = Quaternion.LookRotation(TargetVec);
    }

    bool CanMove()
    {
        bool ret = true;

        ret &= Target;
        ret &= (AtkRange < Dist);

        if(ret)
            Anim.SetBool("IsMove", true);
        else
            Anim.SetBool("IsMove", false);

        return ret;
    }
    
    void Move()
    {
        if (!CanMove()) return;
        Anim.speed = 1.0f;
        TurnToTarget();
        transform.position += TargetVec * Speed * Time.deltaTime;
    }

    bool CanAttack()
    {
        bool ret = true;

        AtkDelay += Time.deltaTime;

        if (Target == null) return false;
        if (AtkRange < Dist) return false;
        if (Alive() == false) return false;
        if((1.0f / AtkSpeed) >= AtkDelay) return false;
        
        return ret;

    }

    void Attack()
    {
        if (CanAttack() == false) return;
        Anim.SetBool("IsMove", false);
        Anim.speed = AtkSpeed;

        TurnToTarget();
        Anim.SetTrigger("DoAttack");
        AtkDelay = 0;
    }

    public void RemoveTarget()
    {
        if(Target != null)
        {
            Monsters.Remove(Target);
            Target = null;
        }
    }

    public void ChangeSkillType()
    {
        if (++SkillType > MaxSkillType - 1)
            SkillType = 0;
        Anim.SetInteger("SkillType", SkillType);
    }

    public void OnTriggerEnter(Collider other)
    {
       if(Alive() == false) return;
       if (other.tag == "MonsterBody" )
        {
            Monster monster = other.GetComponentInParent<Monster>();
            CurHealth -= monster.Atk;
            if (CurHealth < 0)
            {
                CurHealth = 0;
                Die();
            }
        }
    }
 
}
