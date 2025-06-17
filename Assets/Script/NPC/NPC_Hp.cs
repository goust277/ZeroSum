using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Hp : MonoBehaviour, IDamageAble
{
    [Header("Flash")]
    [SerializeField] private DamageFlash flash;

    [Header("Hp_UI")]
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private Image hpImg;

    [Header("Hp")]
    [SerializeField] private int hp;
    [SerializeField] private float immunityTime;
    private float curImmunityTime = 0f;

    public bool immunity = false;
    private float maxHp;
    public bool isDead = false;

    private void Start()
    {
        maxHp = hp;
        hpTxt.text = hp.ToString() + " / " + maxHp.ToString();
    }
    public void Damage(int atk)
    {
        if (!immunity && hp > 0)
        {
            
            hp--;
            if (hp != 0)
            {
                flash.TriggerFlash(immunityTime);
            }
            hpTxt.text = hp.ToString() + " / " + maxHp.ToString();
            hpImg.fillAmount = hp / maxHp;
            immunity = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 3)
        {
            if (hpImg.color != Color.green)
            {
                hpImg.color = Color.green;
            }
        }
        else if (hp <= 3 && hp > 1) 
        {
            if (hpImg.color != Color.yellow)
            {
                hpImg.color = Color.yellow;
            }
        }
        else if (hp <= 1)
        {
            if (hpImg.color != Color.red)
            {
                hpImg.color = Color.red;
            }
        }


        if (immunity)
        {
            curImmunityTime += Time.deltaTime;

            if (curImmunityTime >= immunityTime)
            {
                immunity = false;
                curImmunityTime = 0f;
            }
        }

        if (hp <= 0 && !isDead)
        {
            isDead = true;
            Ver01_DungeonStatManager.Instance.GameOver();
        }

    }
    public void SetHp(int hpAmount)
    {
        hp = hpAmount;
        hpTxt.text = hp.ToString() + " / " + maxHp.ToString();
        hpImg.fillAmount = hp / maxHp;
    }

}
