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
    public int MaxExp = 100;
    public int Exp = 0;
    [Space(10.0f)]
    public int MaxHealth = 100;
    public int CurHealth;
    public int Atk = 10;
    public float AtkSpeed = 1;
    public int AtkRange = 1;
    public float Speed = 10.0f;

    [Space(10.0f)]
    public int SkillType = 0;
    public int MaxSkillType = 3;
    public int Skill2Area = 2;

    
    //public float Atk1CoolTime = 3.0f;

    bool IsMove;
    bool IsDead = false;

    float AtkDelay;
    float Dist;
    Vector3 TargetVec;

    Rigidbody Rigid;
    Animator Anim;

    List<GameObject> Monsters;
    public GameObject Target;
    public BoxCollider WeaponCollision;
    // Start is called before the first frame update
    void Awake()
    {
        CurHealth = MaxHealth;
        Anim = GetComponentInChildren<Animator>();
        Rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Monsters = new List<GameObject>(GameObject.FindGameObjectsWithTag("Monster"));
    }
    
    // Update is called once per frame
    void Update()
    {
        Rigid.velocity = Vector3.zero;
        Rigid.angularVelocity = Vector3.zero;
        Detect();
        Die();
        Move();

        Attack();
    }

    void Detect()
    {
        if (Monsters.Any() && Target == null)
        {
            Dist = (Monsters[0].transform.position - transform.position).sqrMagnitude;
            Target = Monsters[0];
            foreach (GameObject mon in Monsters)
            {
                float d = (mon.transform.position - transform.position).sqrMagnitude;
                if (Dist > d)
                {
                    Dist = d;
                    Target = mon;
                }
            }
        }
        if (Target)
            Dist = (Target.transform.position - transform.position).sqrMagnitude;

        IsDead = CurHealth <= 0;
    }

    void Die()
    {
        if (!IsDead) return;
            Anim.speed = 1.0f;
        Anim.SetTrigger("Die");
        IsDead = true;
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

        ret &= Target;
        ret &= (AtkRange >= Dist);

        AtkDelay += Time.deltaTime;
        ret &= (1.0f / AtkSpeed) < AtkDelay;

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
        Monsters.Remove(Target);
        Target = null;
    }

    public void ChangeSkillType()
    {
        if (++SkillType > MaxSkillType - 1)
            SkillType = 0;
        Anim.SetInteger("SkillType", SkillType);
    }
}
