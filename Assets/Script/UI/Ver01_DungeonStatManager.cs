using Com.LuisPedroFonseca.ProCamera2D;
using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ver01_DungeonStatManager : MonoBehaviour
{
    public static Ver01_DungeonStatManager Instance { get; private set; }

    [Header("HUD Resource")]
    [SerializeField] private TextMeshProUGUI currentMagazineText;

    [Header("HP HUD Resource")]
    [SerializeField] private GameObject hpSlot;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private GameObject painKiller;
    private List<GameObject> hpUI;


    [Header("Stat Resource")]
    [SerializeField] private TextMeshProUGUI totalMagazineText;
    [SerializeField] private TextMeshProUGUI reinforcementText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPrefab;
    [SerializeField] private GameObject hudUI;

    //private bool isRestarted = false;
    private int currentMagazine;
    private int totalMagazine;
    private int reinforcement;
    public const int maxHP = 5;
    [SerializeField] private int currentHP = 10;

    //[SerializeField] private int damage = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드될 때 실행할 함수 등록
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 이 객체를 삭제
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"새로운 씬 : {scene.name}");
        ReadyHPHud();
        ResetDungeonState();
        //isRestarted = false;
    }
    public int GetMaxHP()
    {
        return maxHP;
    }

    public void SetCurrentHP(int hp)
    {
        currentHP = hp;
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }


    public GameObject GetPainKiller()
    {
        return painKiller;
    }

    private void ReadyHPHud()
    {
        hpUI = new List<GameObject>();

        for (int i = 0; i < hpSlot.transform.childCount; i++)
        {
            //Debug.Log("hpSlot.transform.GetChild(i)" + hpSlot.transform.GetChild(i).name);
            hpUI.Add(hpSlot.transform.GetChild(i).gameObject);
        }
    }

    public void ResetDungeonState()
    {
        //isRestarted = true;
        hudUI.SetActive(true);
        GameStateManager.Instance.resetReinforcement();
        currentMagazine = GameStateManager.Instance.GetTotalMagazine();
        UpdateHUD();
        UpdateHPUI(10);
    }

    public int TakeReloadItem()
    {
        int bullet = Random.Range(5, 20);

        if( currentMagazine + bullet > totalMagazine)
        {
            currentMagazine = totalMagazine;
        }
        else
        {
            currentMagazine += bullet;
        }
        UpdateHUD();
        return bullet;
    }

    public int GetCurrentMagazine()
    {
        return currentMagazine;
    }

    public bool ShotGun()
    {
        currentMagazine --;
        if (currentMagazine < 0) {
            Debug.Log("currentMagazine out");
            currentMagazine = 0;
            return false;
        }
        UpdateHUD();
        return true;
    }

    public void UpdateHUD()
    {
        totalMagazine = GameStateManager.Instance.GetTotalMagazine();
        reinforcement = GameStateManager.Instance.GetReinforcement();

        if (currentMagazineText != null) currentMagazineText.text = currentMagazine.ToString();
    }

    public void UpdateHPUI(int hp)
    {
        if (hpUI != null)
        {
            for (int i = 0; i < hpUI.Count; i++)
            {
                if (i < hp)
                {
                    hpUI[i].gameObject.GetComponent<Image>().sprite = defaultSprite;
                }
                else
                {
                    hpUI[i].gameObject.GetComponent<Image>().sprite = emptySprite;
                }
            }
        }

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 해제
    }

    public void GameOver()
    {
        if (gameOverPrefab == null)
        {
            Debug.LogError("GameOver 프리팹이 할당되지 않았습니다.");
            return;
        }

        hudUI.SetActive(false);
        Instantiate(gameOverPrefab);

    }
}
