using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("HUD Resource")]
    [SerializeField] private GameObject[] hpUI;
    [SerializeField] private GameObject painKiller;
    private TextMeshProUGUI timeText;

    [Header("invincibility time")]
    public float invincibilityTime;
    private int hp = 5;
    private bool isBlocked = false;
    void Start()
    {
        // 자식에서 TextMeshProUGUI 찾기
        timeText = painKiller.GetComponentInChildren<TextMeshProUGUI>();
        painKiller.SetActive(false);
        if (timeText == null)
        {
            Debug.Log("못찾");
        }
    
    }

    public void Death()
    {
        hp = 0;
    }
        //take Damage
    public void Damage()
    {
        if (!isBlocked)
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

    public void GetPainKiller(float blockDuration)
    {
        if (!isBlocked)
        {
            StartCoroutine(BlockFunctionTemporarily(blockDuration));
        }
    }

    private IEnumerator BlockFunctionTemporarily(float duration)
    {
        if (isBlocked) yield break;

        float time = duration;

        painKiller.SetActive(true);
        isBlocked = true;
        while (time > 0.1f)
        {
            timeText.text = time.ToString();
            time -= 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
        
        isBlocked = false;
        painKiller.SetActive(false);
    }

    public void UpdateUI()
    {
        if(hpUI != null)
        {
            for (int i = 0; i < hpUI.Length; i++)
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
            //Ver01_DungeonStatManager.Instance.GameOver();
        }
    }
}
