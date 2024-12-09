using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerSwordAttack sword;
    

    private void Start()
    {
        sword = GetComponent<PlayerSwordAttack>();
    }

    public void OnAttack(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            sword.ComboAttack();
        }
    }
}
