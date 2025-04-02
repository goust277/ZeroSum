using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAtkEnd : MonoBehaviour
{
    [SerializeField] private PlayerGunAttack playerGunAttack;
    [SerializeField] private PlayerSwordAttack playerSwordAttack;
    [SerializeField] private Animator animator;

    public void IsAtkEnd()
    {
        playerGunAttack.isAttack = false;
    }
    
    public void IsSwordAtkEnd()
    {
        if(!playerSwordAttack.isAtk2)
            playerSwordAttack.isAttack = false;
    }
    public void IsSwordAtkStart()
    {
        playerSwordAttack.isAttack = true;
        playerSwordAttack.isAtk2 = false;
    }
    public void IsSwordComboTrue()
    {
        playerSwordAttack.canCombo = true;
    }

    public void IsSwordComboFalse()
    {
        playerSwordAttack.canCombo = false;
    }
}
