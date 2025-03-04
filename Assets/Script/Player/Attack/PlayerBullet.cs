using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public float speed;

    [SerializeField] private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDriection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive") || collision.CompareTag("Monster") || collision.CompareTag("Wall"))
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
            gameObject.SetActive(false);
        }
    }
}
