using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerHit : MonoBehaviour , IDamageAble
{
    [SerializeField] private PlayerHP player;
    [SerializeField] private PlayerMovement playerMovement;
    private bool isInvincibility;

    private float time;
    private void Start()
    {
        isInvincibility = false;
        time = 0f;
    }

    private void Update()
    {
        if (isInvincibility)
        {
            time += Time.deltaTime;

            if (time > player.invincibilityTime )
            {
                isInvincibility = false;
            }
        }
    }
    public void Damage(int value)
    {
        if (!isInvincibility && value != 10)
        {

            player.Damage();
            isInvincibility = true;
            time = 0f;
        }

        if(value == 10 && playerMovement.isGrounded)

        {
            player.InstantDeath();
        }
    }
}
