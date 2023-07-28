using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public BoxCollider WeaponCollision;
    public Rigidbody Rigid;
    Player Owner;

    void Awake()
    {
        Owner = GetComponentInParent<Player>();  
    }

    void Update()
    {
        
    }

    public void OnAtk1TriggerEnable()
    {
        WeaponCollision.enabled = true;
    }

    public void OnAtk1TriggerDisable()
    {
        WeaponCollision.enabled = false;
    }

    public void OnAtk1Trigger()
    {
        if (Owner.SkillType == 0)
        {
            Monster mon = Owner.Target.GetComponent<Monster>();
            mon.ApplyDamage(Owner.Atk, Owner);
        }
        Owner.ChangeSkillType();

    }

    public void OnAtk2Trigger()
    {
        RaycastHit[] RayHits = Physics.SphereCastAll(Owner.Target.transform.position,
            Owner.Skill2Area, Vector3.up, 0f, LayerMask.GetMask("Monster"));

        foreach(RaycastHit hitobj in RayHits) 
        {
            hitobj.transform.GetComponent<Monster>().ApplyDamage(Owner.Atk, Owner);
        }
        Owner.ChangeSkillType();

    }

    public void OnAtk3Trigger()
    {
        Owner.CurHealth += Owner.Atk;
        if(Owner.CurHealth >= Owner.MaxHealth) { Owner.CurHealth = Owner.MaxHealth; }
        Owner.ChangeSkillType();

    }

    public void OnAttackEnd()
    {
        //Owner.ChangeSkillType();
    }

    public void OnDeadEnd()
    {
        Owner.OnDeadEnd();
    }
}
