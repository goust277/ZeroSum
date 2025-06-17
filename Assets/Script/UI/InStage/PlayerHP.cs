using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHP : MonoBehaviour
{
    //[Header("HUD Resource")]
    //[SerializeField] 
    private GameObject painKiller;
    private TextMeshProUGUI timeText;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimation playerAnimation;
    private AudioSource externalAudioSource;

    [Header("invincibility time")]
    public float invincibilityTime;
    [SerializeField] private float curInvincibillityTime = 0f;
    public bool isInvincibility;

    [Header("Hp")]
    public int hp = 10;
    private int maxHp;

    public bool isBlocked = false;

    [Header("Flash")]
    [SerializeField] private DamageFlash flash;

    [Header("Dying")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject col;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveTime;


    private float curMoveTime;
    private Rigidbody2D rb;
    public event Action OnDying;
    private bool OnDeath;
    private Vector3 deathDir;

    private bool isPainKillerActive = false;
    private float painKillerTimer = 0f;

    void Start()
    {
        // �ڽĿ��� TextMeshProUGUI ã��
        painKiller = Ver01_DungeonStatManager.Instance.GetPainKiller();
        timeText = painKiller.GetComponentInChildren<TextMeshProUGUI>();

        painKiller.SetActive(false);

        OnDeath = false;
        rb = GetComponent<Rigidbody2D>();

        maxHp = Ver01_DungeonStatManager.Instance.GetMaxHP();
        Ver01_DungeonStatManager.Instance.UpdateHPUI(maxHp);
        Ver01_DungeonStatManager.Instance.SetCurrentHP(maxHp);
        hp = maxHp;

        curMoveTime = 0f;

        //오디오 세팅
        if (externalAudioSource == null)
        {
            GameObject audioManager = GameObject.Find("AudioManager");
            if (audioManager == null)
            {
                Debug.LogWarning("AudioManager 오브젝트 xxxx");
            }

            Transform itemChild = audioManager.transform.Find("Hit");
            externalAudioSource = itemChild.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (OnDeath)
        {
            curMoveTime += Time.deltaTime;
            if(curMoveTime < moveTime)
            {
                rb.velocity = deathDir * moveSpeed;
            }
            else if (curMoveTime >= moveTime)
            {
                rb.velocity = Vector2.zero;
            }
        }

        if (curInvincibillityTime > 0f)
        {
            curInvincibillityTime -= Time.deltaTime;
        }
        else if (curInvincibillityTime <= 0f && isInvincibility && !playerMovement.isDashing)
        {
            isInvincibility = false;
        }

        if (isPainKillerActive)
        {
            painKillerTimer -= Time.deltaTime;

            timeText.text = Mathf.CeilToInt(painKillerTimer).ToString();

            if (painKillerTimer <= 0f)
            {
                isPainKillerActive = false;
                isBlocked = false;
                painKiller.SetActive(false);
            }
        }
    }
    public void InstantDeath()
    {
        hp = 0;
        Ver01_DungeonStatManager.Instance.SetCurrentHP(0);
        Ver01_DungeonStatManager.Instance.UpdateHPUI(hp);

        Debug.Log("Game Over");

        GameStateManager.Instance.UseReinforcement();
        int reinforcement = GameStateManager.Instance.GetReinforcement();
        HandleDeath(reinforcement);
    }
    //take Damage
    public void Damage()
    {
        if (!isInvincibility)
        {
            isInvincibility = true;
            curInvincibillityTime = invincibilityTime;

            externalAudioSource.Play();
            if (!isBlocked)
            {
                hp--;

                if (hp <= 0 && !OnDeath)
                {
                    Debug.Log("Die");
                    GameStateManager.Instance.UseReinforcement();
                    int reinforcement = GameStateManager.Instance.GetReinforcement();
                    HandleDeath(reinforcement);
                    return;
                }

                if (hp > 0)
                {
                    flash.TriggerFlash(invincibilityTime);
                    Ver01_DungeonStatManager.Instance.SetCurrentHP(hp);
                    Ver01_DungeonStatManager.Instance.UpdateHPUI(hp);
                }
            }

            if(!isPainKillerActive)
            {
                playerAnimation.Hit();
            }
        }
    }

    public void GetHPItem()
    {
        Debug.Log("GetHPItem 호출");
        hp = Ver01_DungeonStatManager.Instance.GetCurrentHP();
        hp++;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        Ver01_DungeonStatManager.Instance.SetCurrentHP(hp);
        Ver01_DungeonStatManager.Instance.UpdateHPUI(hp);
    }

    public void GetPainKiller(float blockDuration)
    {
        Debug.Log("GetPainKiller 호출");

        painKiller.SetActive(true);
        isBlocked = true;
        isPainKillerActive = true;
        painKillerTimer = blockDuration;
    }

    private void HandleDeath(int reinforcement)
    {
        //if (reinforcement > 0)
        //{
        //    return;
        //}
        //else // die
        //{
        Debug.Log("Game Over");
        playerInput.enabled = false;
        col.SetActive(false);
        OnDeath = true;
        OnDying?.Invoke();
        rb.bodyType = RigidbodyType2D.Kinematic;

        if(playerMovement.moveLeft)
        {
            deathDir = new Vector3(1f, 0f, 0f);
        }
        else
        {
            deathDir = new Vector3(-1f, 0f, 0f);
        }
        Ver01_DungeonStatManager.Instance.GameOver();
        //}
    }

    public void ContinueProcessing(float blockDuration)
    {
        Debug.Log("PlayerHP - ContinueProcessing 실행중");

        playerInput.enabled = true;
        col.SetActive(true);
        OnDeath = false;
        rb.bodyType = RigidbodyType2D.Dynamic;

        // blockDuration 만큼 무적
        isPainKillerActive = true;
        isBlocked = true;
        painKillerTimer = blockDuration;
        flash.TriggerFlash(blockDuration);

        playerAnimation.Resurrection();
        hp = Ver01_DungeonStatManager.Instance.GetCurrentHP();
    } 
}
