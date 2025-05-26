using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timer = 2.5f;
    public Sprite next;

    private void Start()
    {
        Destroy(gameObject, timer);
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0.5f)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = next;
        }
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
