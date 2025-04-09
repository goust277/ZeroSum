using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timer = 5f;

    private void Start()
    {
        Destroy(gameObject, timer);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Plane") || other.CompareTag("Wall"))
        {
            if (this.CompareTag("MonsterAtk"))
            {
                IDamageAble damageable = other.GetComponent<IDamageAble>();
                damageable?.Damage(1);
                Destroy(gameObject);
            }
        }
    }
}
