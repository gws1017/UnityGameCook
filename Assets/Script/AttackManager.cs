using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    // Start is called before the first frame update
    public BoxCollider WeaponCollision;
    public Player Owner;
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

    public void OnAttackEnd()
    {
        Owner.ChangeSkillType();
    }
    
}
