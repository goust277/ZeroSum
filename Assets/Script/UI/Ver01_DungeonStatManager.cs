using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
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
    private List<GameObject> hpUI;

    private int currentMagazine;
    private int totalMagazine;
    private int reinforcement;

    //[SerializeField] private int damage = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  // 싱글톤 인스턴스 초기화
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 이 객체를 삭제
        }
    }


    private void Start()
    {
        ReadyHPHud();
        ResetDungeonState();
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
        currentMagazine = GameStateManager.Instance.GetTotalMagazine();
        UpdateHUD();
    }

    //public int GetDamageValue()
    //{
    //    return damage;
    //}

    public int TakeReloadItem()
    {
        int bullet = Random.Range(1, 4);
        currentMagazine += bullet;
        UpdateHUD();
        return bullet;
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


    public void GameOver()
    {
        //Gameover Function 

        //최종 세이브 위치 저장
        //저장했을때 있었던 스탯들 저장
        //. 씬 전환 (전환하더라도 계속 따라감)
        //게임오버 화면에서 컨티뉴 누르면 다시 불러오기 << 이건게임오버화면 컨트롤러같은거 필요한가?
    }

}
