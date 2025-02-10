using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Rigidbody2D p;
    Vector2 knockbackForce = new Vector2(0, 5f);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            p.AddForce(knockbackForce, ForceMode2D.Impulse);
        }
    }
}
