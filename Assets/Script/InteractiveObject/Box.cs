using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private float hitCount;
    void Start()
    {
        hitCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsHit()
    {
        hitCount++;
    }
}
