using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Check : MonoBehaviour
{
    public Melee m;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // "Wall" 태그가 있는 오브젝트만 감지
        if (other.CompareTag("Wall"))
        {
            m.FlipTarget();
        }
    }
}
