using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAtkEnd : MonoBehaviour
{
    [SerializeField] private PlayerGunAttack playerGunAttack;
    [SerializeField] private PlayerSwordAttack playerSwordAttack;

    public void IsAtkEnd()
    {
        playerGunAttack.isAttack = false;
    }
    
    public void IsSwordAtkEnd()
    {
        playerSwordAttack.isAttack = false;
    }

}
