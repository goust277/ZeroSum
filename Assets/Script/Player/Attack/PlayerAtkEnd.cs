using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAtkEnd : MonoBehaviour
{
    [SerializeField] private PlayerGunAttack playerGunAttack;

    public void IsAtkEnd()
    {
        playerGunAttack.isAtkEnd = false;
    }

}
