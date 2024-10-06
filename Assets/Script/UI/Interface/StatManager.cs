using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IHealth
{
    public static HealthManager Instance { get; private set; }

    public float MaxHP { get; set; }
    public float CurrentHP { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 바뀌어도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 플레이어가 대미지를 받으면 대미지 값을 전달 받아 HP에 반영
    public void GetDamage(int damage, Vector2 attackerPosition)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Destroy(gameObject);
            //뭐 사망 모션.. . 등등
        }
        // 피격당한 방향 계산 (적 -> 플레이어의 반대 방향으로 밀려남)
        Vector2 knockbackDirection = ((Vector2)transform.position - attackerPosition).normalized;

        //넉백 힘 임의설정
        float knockBackPower = 5.0f;
        //적용된 오브젝트Rigidbody2D에 힘을 가해 밀려나게 함
        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockBackPower, ForceMode2D.Impulse);
    }
}
