using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Ver01_DungeonStatManager : MonoBehaviour
{
    [Header("HUD Resource")]
    [SerializeField] private TextMeshProUGUI currentMagazineText;
    [SerializeField] private GameObject[] hpUI;

    private int currentMagazine;
    private int totalMagazine;
    private int hp;
    private int reinforcement;

    private void Start()
    {
        ResetDungeonState();
    }

    public void ResetDungeonState()
    {
        hp = 5;
        currentMagazine = 0;
        UpdateHUD();
    }

    public void GetHPItem()
    {
        hp++;
        UpdateHUD();
    }

    public void GetReloadItem()
    {
        currentMagazine += Random.Range(1, 4);
        UpdateHUD();
    }

    public void TakeDamage()
    {
        if (hp-1 < 0)
        {
            GameStateManager.Instance.UseReinforcement();
            UpdateHUD();
            //HandleDeath();
        }
        else
        {
            hp--;
            UpdateHUD();
        }
    }

    public void UpdateHUD()
    {
        totalMagazine = GameStateManager.Instance.GetTotalMagazine();
        reinforcement = GameStateManager.Instance.GetReinforcement();

        if (currentMagazineText != null) currentMagazineText.text = currentMagazine.ToString();
        //if (hpText != null) hpText.text = hp.ToString();
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
    private void HandleDeath()
    {

        if (reinforcement > 0)
        {
            //hp = 5;  // 강화 단계 사용 후 HP 복구
            UpdateHUD();
        }
        else
        {
            Debug.Log("Game Over");
        }
    }

}
