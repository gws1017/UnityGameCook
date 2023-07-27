using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    // Start is called before the first frame update
    public BoxCollider WeaponCollision;
    public Rigidbody Rigid;
    Player Owner;

    void Awake()
    {
        Owner = GetComponentInParent<Player>();  
    }
    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void OnAreaAttack()
    {
        RaycastHit[] RayHits = Physics.SphereCastAll(Owner.Target.transform.position,
            Owner.Skill2Area, Vector3.up, 0f, LayerMask.GetMask("Monster"));

        foreach(RaycastHit hitobj in RayHits) 
        {
            hitobj.transform.GetComponent<Monster>().HitFromAtk2(Owner);
        }
    }

    public void OnHealSkill()
    {
        Owner.CurHealth += Owner.Atk;
        if(Owner.CurHealth >= Owner.MaxHealth) { Owner.CurHealth = Owner.MaxHealth; }
    }

    public void OnAttackEnd()
    {
        Owner.ChangeSkillType();
    }
    
}
