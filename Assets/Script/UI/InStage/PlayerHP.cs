using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour, IDamageAble
{
    [Header("HUD Resource")]
    [SerializeField] private GameObject[] hpUI;

    private int hp;
    //take Damage
    public void Damage(int value)
    {
        if (hp - 1 < 0)
        {
            GameStateManager.Instance.UseReinforcement();
            int reinforcement = GameStateManager.Instance.GetReinforcement();
            HandleDeath(reinforcement);
        }
        else
        {
            hp--;
            UpdateUI();
        }
    }

    public void GetHPItem()
    {
        hp++;
        if (hp > 5)
        {
            hp = 5;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(hpUI is null)
        {
            return;
        }
        for (int i = 0; i < 5; i++)
        {
            if (i < hp)
            {
                hpUI[i].SetActive(true);
            }
            else
            {
                hpUI[i].SetActive(false);
            }
        }
    }

    private void HandleDeath(int reinforcement)
    {
        if (reinforcement > 0)
        {
            return;
        }
        else // die
        {
            Debug.Log("Game Over");
            Ver01_DungeonStatManager.Instance.GameOver();
        }
    }
}
