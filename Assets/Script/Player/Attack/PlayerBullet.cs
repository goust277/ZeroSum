using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public float speed;

    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDriection(Vector2 dir)
    {
        direction = dir.normalized;
    }
}
