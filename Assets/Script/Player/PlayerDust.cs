using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDust : MonoBehaviour
{
    [SerializeField] private Animator dust;
    
    public void OnDust()
    {
        dust.SetTrigger("Dust");
    }
}
