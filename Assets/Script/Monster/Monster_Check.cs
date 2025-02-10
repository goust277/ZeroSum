using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Check : MonoBehaviour
{
    public Monster_Spawner spawner;
    public GameObject player;

    public void Initialize(Monster_Spawner spawner, GameObject player)
    {
        this.spawner = spawner;
        this.player = player;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.RemoveMonsterFromList(gameObject);
        }
    }
}