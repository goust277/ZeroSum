using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Attack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            IDamageAble damageable = other.GetComponent<IDamageAble>();
            if (damageable != null)
            {
                damageable.Damage(10);
            }
        }
    }
}
