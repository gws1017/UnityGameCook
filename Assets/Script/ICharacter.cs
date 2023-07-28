using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public void OnDeadEnd();

    public void OnAttackEnd();
    public void OnAtkColliderEnable();
    public void OnAtkColliderDisable();

    public void OnAtkTrigger();
}
