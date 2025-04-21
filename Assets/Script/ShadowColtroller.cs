using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowColtroller : MonoBehaviour
{
    [SerializeField] private Shadow[] shadows;
    [SerializeField] private float time;

    public bool isInPlayer = false;

    private int inPlayerCount = 0;
    // Update is called once per frame
    void Update()
    {
        if (isInPlayer)
        {
            foreach (var shadow in shadows)
            {
                shadow.InPlayer(time);
            }
        }
        else
        {
            foreach (var shadow in shadows)
            {
                shadow.OutPlayer(time);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInPlayer = false;
        }
    }
}
