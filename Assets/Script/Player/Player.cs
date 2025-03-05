using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageAble
{
    private bool isHit;
    private bool isHitPossible;
    [SerializeField] private int hp;
    [SerializeField] private float InvincibilityTime;

    private float Invincibility;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            Invincibility -= Time.deltaTime;

            if (Invincibility <= 0)
                isHit = false;
        }
    }
    public void Damage(int atk)
    {
        if (isHit)
        {
            return;
        }

        if (hp <= 0)
        {

        }
        else if (hp > 0 && !isHit)
        {
            Debug.Log("플레이어 히트");
            hp -= atk;
            isHit = true;
        }
    }
}
