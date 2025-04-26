using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_atk : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && this.CompareTag("MonsterAtk"))
        {
            IDamageAble damageable = other.GetComponent<IDamageAble>();
            damageable?.Damage(1);
        }
    }
}
