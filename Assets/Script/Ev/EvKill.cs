using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvKill : MonoBehaviour
{
    [SerializeField] private BaseEv movingBlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.CompareTag("MovingBlock"))
        {
            if (movingBlock.isBottom && movingBlock.isMoving)
            {
                IDamageAble damageAble = collision.GetComponent<IDamageAble>();
                if (damageAble != null)
                {
                    damageAble.Damage(10);
                }
            }
        }
    }
}
