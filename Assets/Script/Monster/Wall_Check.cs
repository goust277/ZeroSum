using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Check : MonoBehaviour
{
    public Melee m;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // "Wall" �±װ� �ִ� ������Ʈ�� ����
        if (other.CompareTag("Wall"))
        {
            m.FlipTarget();
        }
    }
}
