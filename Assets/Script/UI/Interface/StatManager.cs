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
            DontDestroyOnLoad(gameObject);  // ���� �ٲ� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �÷��̾ ������� ������ ����� ���� ���� �޾� HP�� �ݿ�
    public void GetDamage(int damage, Vector2 attackerPosition)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Destroy(gameObject);
            //�� ��� ���.. . ���
        }
        // �ǰݴ��� ���� ��� (�� -> �÷��̾��� �ݴ� �������� �з���)
        Vector2 knockbackDirection = ((Vector2)transform.position - attackerPosition).normalized;

        //�˹� �� ���Ǽ���
        float knockBackPower = 5.0f;
        //����� ������ƮRigidbody2D�� ���� ���� �з����� ��
        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockBackPower, ForceMode2D.Impulse);
    }
}
