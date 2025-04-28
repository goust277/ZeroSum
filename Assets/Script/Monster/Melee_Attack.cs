using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Attack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.CompareTag("MonsterAtk"))
            {
                IDamageAble damageable = other.GetComponent<IDamageAble>();
                if (damageable != null)
                {
                    try
                    {
                        Debug.Log($"�浹�� ������Ʈ �̸�: {other.gameObject.name}");
                        damageable.Damage(1);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Damage() ���� �߻�! {e.Message}\n{e.StackTrace}");
                    }
                }
                else
                {
                    Debug.LogWarning("IDamageAble ������Ʈ ����");
                }
            }
        }
    }
}
