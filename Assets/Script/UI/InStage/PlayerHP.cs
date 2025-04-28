using System;
using System.Collections;
using TMPro;
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
    [Header("invincibility time")]
    public float invincibilityTime;
    [HideInInspector] public int hp;
    private bool isBlocked = false;

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
    void Start()
    {
        // �ڽĿ��� TextMeshProUGUI ã��
        painKiller = Ver01_DungeonStatManager.Instance.GetPainKiller();
        timeText = painKiller.GetComponentInChildren<TextMeshProUGUI>();

        painKiller.SetActive(false);

        OnDeath = false;
        rb = GetComponent<Rigidbody2D>();

        hp = Ver01_DungeonStatManager.maxHP;
        Ver01_DungeonStatManager.Instance.SetCurrentHP(hp);

        curMoveTime = 0f;
    }


    private void Update()
    {
        if(OnDeath)
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
        hp = Ver01_DungeonStatManager.Instance.GetCurrentHP();

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
                playerAnimation.Hit();
                Debug.Log("Hit");
                hp--;
                Ver01_DungeonStatManager.Instance.UpdateHPUI(hp);
            }
        }
    }

    public void GetHPItem()
    {
        Debug.Log("GetHPItem 호출");

        hp++;
        if (hp > Ver01_DungeonStatManager.maxHP)
        {
            hp = Ver01_DungeonStatManager.maxHP;
        }
        Ver01_DungeonStatManager.Instance.SetCurrentHP(hp);
        Ver01_DungeonStatManager.Instance.UpdateHPUI(hp);
    }

    public void GetPainKiller(float blockDuration)
    {
        Debug.Log("GetPainKiller 호출");

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
        }
    }
}
