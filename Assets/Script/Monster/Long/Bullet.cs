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
                        Debug.Log("Damage 호출 전");
                        damageable.Damage(1);
                        Debug.Log("Damage 호출 후"); // 이게 안 나오고 있음
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Damage() 예외 발생! {e.Message}\n{e.StackTrace}");
                    }
                }
                else
                {
                    Debug.LogWarning("IDamageAble 컴포넌트 없음");
                }
            }
            Destroy(gameObject);
        }
    }
}
