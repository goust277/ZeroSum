using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("HUD Resource")]
    [SerializeField] private GameObject painKiller;
    private TextMeshProUGUI timeText;
    
    [Header("invincibility time")]
    public float invincibilityTime;
    [HideInInspector] public int hp = 10;
    private int maxhp = 10;
    private bool isBlocked = false;

    [Header("Dying")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject col;
    private Rigidbody2D rb;
    public event Action OnDying;
    private bool OnDeath;
    void Start()
    {
        // �ڽĿ��� TextMeshProUGUI ã��
        timeText = painKiller.GetComponentInChildren<TextMeshProUGUI>();

        painKiller.SetActive(false);
        if (timeText == null)
        {
            Debug.Log("��ã");
        }

        OnDeath = false;
        rb = GetComponent<Rigidbody2D>();
    }

    //[Obsolete]
    //private void Update()
    //{
    //    if (hp <= 0)
    //    {
    //        if(!OnDeath)
    //        {
    //            playerInput.enabled = false;
    //            col.SetActive(false);
    //            OnDeath = true;
    //            OnDying?.Invoke();
    //            rb.velocity = Vector3.zero;
    //            rb.bodyType = RigidbodyType2D.Kinematic;
    //        }
    //    }
    //}
    public void InstantDeath()
    {
        hp = 0;
        Ver01_DungeonStatManager.Instance.UpdateHPUI(hp);

        Debug.Log("Game Over");

        GameStateManager.Instance.UseReinforcement();
        int reinforcement = GameStateManager.Instance.GetReinforcement();
        HandleDeath(reinforcement);
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
                Debug.Log("Hit");
                hp--;
                Ver01_DungeonStatManager.Instance.UpdateHPUI(hp);
            }
        }
    }

    public void GetHPItem()
    {
        hp++;
        if (hp > maxhp)
        {
            hp = maxhp;
        }
        Ver01_DungeonStatManager.Instance.UpdateHPUI(hp);
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

    private void HandleDeath(int reinforcement)
    {
        if (reinforcement > 0)
        {
            return;
        }
        else // die
        {
            Debug.Log("Game Over");
            playerInput.enabled = false;
            col.SetActive(false);
            OnDeath = true;
            OnDying?.Invoke();
            rb.velocity = Vector3.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;

            //Ver01_DungeonStatManager.Instance.GameOver();
        }
    }
}
