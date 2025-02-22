using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerSwordAttack sword;
    private PlayerGunAttack gun;


    private void Start()
    {
        sword = GetComponent<PlayerSwordAttack>();
        gun = GetComponent<PlayerGunAttack>();
    }

    public void OnAttack(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            gun.GunAttack();
        }
    }
}
