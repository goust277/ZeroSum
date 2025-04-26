using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_Damage : MonoBehaviour,IDamageAble
{
    public WorldTree boss;
    public int maxHp = 5;
    private int currentHp;
    private bool destroyed = false;

    void Start()
    {
        currentHp = maxHp;
    }

    public void Damage(int atk)
    {
        if (destroyed || boss.headExposed) return;

        currentHp --;

        if (currentHp <= 0)
        {
            destroyed = true;
            boss.DestroyRightArm();  // ������ �˸� �� Ÿ�̸� ���� �� ���� ���� ���� üũ
        }
        else
        {
            boss.Damage(1);  // ��ü �������� �ݿ�
        }
    }

    public void ResetArm()
    {
        destroyed = false;
        boss.rightArmDamage.GetComponent<Collider2D>().enabled = true;
        var renderer = boss.Right_atk.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color c = renderer.color;
            c.a = 1f; 
            renderer.color = c;
        }
        currentHp = maxHp;
    }
}
