using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    ICharacter Owner;

    void Awake()
    {
        Owner = GetComponentInParent<ICharacter>();  
    }

    void Update()
    {
        
    }

    public void OnAtk1TriggerEnable()
    {
        Owner.OnAtkColliderEnable();
    }

    public void OnAtk1TriggerDisable()
    {
        Owner.OnAtkColliderDisable();
    }

    public void OnAtkTrigger()
    {
        Owner.OnAtkTrigger();
    }

    public void OnAttackEnd()
    {
        //Owner.ChangeSkillType();
        Owner.OnAttackEnd();
    }

    public void OnDeadEnd()
    {
        Owner.OnDeadEnd();
    }
}
