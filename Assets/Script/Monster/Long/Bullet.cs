using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timer = 2.5f;

    private void Start()
    {
        Destroy(gameObject, timer);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ground") || other.CompareTag("Wall"))
        {
            if (this.CompareTag("MonsterAtk"))
            {
                IDamageAble damageable = other.GetComponent<IDamageAble>();
                if (damageable != null)
                {
                    damageable.Damage(1);
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
