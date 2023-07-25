using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stat")]
    public int Level = 1;
    public int MaxExp = 100;
    public int Exp = 0;
    [Space(10.0f)]
    public int MaxHealth = 100;
    public int CurrentHealth;
    public int Atk = 10;
    public int AtkSpeed = 1;
    public int AtkRange = 1;
    public float Speed = 10.0f;

    [Space(10.0f)]
    public int SkillType = 0;
    public int MaxSkillType = 3;
    public int Skill2Area = 2;

    
    //public float Atk1CoolTime = 3.0f;

    bool IsMove;
    bool IsAtkReady = true;
    bool IsDead = false;

    float AtkDelay;

    Animator Anim;

    float Dist;
    List<GameObject> Monsters;
    GameObject Target;
    // Start is called before the first frame update
    void Awake()
    {
        CurrentHealth = MaxHealth;
        Anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        Monsters = new List<GameObject>(GameObject.FindGameObjectsWithTag("Monster"));
    }
    
    // Update is called once per frame
    void Update()
    {
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

        IsDead = CurrentHealth <= 0;
    }

    void Die()
    {
        if (!IsDead) return;
            Anim.speed = 1.0f;
        Anim.SetTrigger("Die");
        IsDead = true;
        CurrentHealth = MaxHealth;
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

        Vector3 MoveVec = (Target.transform.position - transform.position);
        MoveVec.y = 0;
        MoveVec.Normalize();
        transform.position += MoveVec * Speed * Time.deltaTime;
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

        Anim.SetTrigger("DoAttack");
        SkillType++;
        if (SkillType > MaxSkillType - 1)
            SkillType = 0;
        Anim.SetInteger("SkillType", SkillType);
        AtkDelay = 0;

    }
}
