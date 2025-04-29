using Com.LuisPedroFonseca.ProCamera2D;
using System;
using UnityEngine;

public class PlayerSwordAttack : PlayerAttackState
{
    [SerializeField] private float delay;
    [SerializeField] private float atkCoolTime;
    
    [HideInInspector] public bool isParryingReady;

    private bool isAtkReady;
    public bool canCombo = true;
    [HideInInspector] public bool isAtk2;

    public event Action OnSwordAttack;
    public event Action OnSwordSecAttack;
    
    private void Start()
    {
        isAtkReady = true;
        canCombo = true ;
        isAtk2 = false;
        delay = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAtkReady)
        {
            delay += Time.deltaTime;
            if (atkCoolTime <= delay)
            {
                isAtkReady = true;
            }
        }
        else
        {
            if (delay != 0f)
            {
                delay = 0f;
            }
        }
    }
    public void OnAttack()
    {
        Debug.Log("АјАн");
        if (isAtkReady)
        {
            isAttack = true;

            if (canCombo)
            {
                OnSwordAttack?.Invoke();
            }
            else
            {
                OnSwordSecAttack?.Invoke();
                isAtk2 = true;
            }
        }
    }

    public void Parrying()
    {
        delay = attackDelay;
        isAttack = false;
        isAtkReady = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive") || collision.CompareTag("Monster"))
        {
            IDamageAble damageable = collision.GetComponent<IDamageAble>();
            if (damageable != null)
            {
                damageable.Damage(damage);
            }

            Bomb bomb = collision.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.TakeDamage(transform.position);
            }

            var shakePreset = ProCamera2DShake.Instance.ShakePresets[0];

            ProCamera2DShake.Instance.Shake(shakePreset);
        }
    }
}
