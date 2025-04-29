
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;
    public bool isAttack;
}
