using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timer = 5f;

    private void Start()
    {
        Destroy(gameObject, timer);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Plane") || other.CompareTag("Wall"))
        {
            if (this.CompareTag("MonsterAtk"))
            {
                IDamageAble damageable = other.GetComponent<IDamageAble>();
                if (damageable != null)
                {
                    try
                    {
                        Debug.Log("Damage ȣ�� ��");
                        damageable.Damage(1);
                        Debug.Log("Damage ȣ�� ��"); // �̰� �� ������ ����
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
            Destroy(gameObject);
        }
    }
}
