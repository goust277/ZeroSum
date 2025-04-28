using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Damage : MonoBehaviour, IDamageAble
{
    public WorldTree boss;
    public void Damage(int atk)
    {
        if (boss.headExposed)
            boss.Damage(2);
    }
}
