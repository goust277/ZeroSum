using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using System.Reflection;


public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }


    public int[] activeWeapons = new int[2]; // 현재 적용중인 무기의 ID 배열
    [SerializeField] private int currentActiveLayer; // 1 혹은 2
    [SerializeField] private List<int> acquiredWeaponIds; // 플레이어가 얻은 무기 리스트
    private List<Weapon> allWeapons = new List<Weapon>(); // 모든 칩셋 리스트

    public RuntimeAnimatorController originalAnimatorController;
    private AnimatorOverrideController currentOverrideController;
    private Animator animator;

    [SerializeField] private GameObject[] activeWeaponObject = new GameObject[2];


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환해도 유지
        }
        else
        {
            Destroy(gameObject);
        }

        LoadData("Ver00/Dataset/Weapons.json"); // 무기 데이터 로드

    }

    private void Start()
    {
        animator = GameObject.FindWithTag("Player")?.GetComponent<Animator>();
        currentActiveLayer = 1;
    }

    private void LoadData(string path)
    {
        TextAsset weaponJson = Resources.Load<TextAsset>("Json/Ver00/Dataset/Weapons");

        //string basePath = Application.dataPath + "/Resources/Json/";
        //string fullPath = basePath + path;

        //string jsonData = File.ReadAllText(fullPath);
        string jsonData = weaponJson.text;


        // T는 아이템 배열로 변환
        WeaponsArray items = JsonConvert.DeserializeObject<WeaponsArray>(jsonData);

        // items 객체에서 Items라는 프로퍼티를 찾아 그 값을 가져와
        //이 값이 IEnumerable<T> 타입으로 변환 가능한 경우 각 요소를 순회하며 item으로 사용
        foreach (var item in items.Items)
        {
            allWeapons.Add(item);
        }
    }

    private void AddAnimationOverride(List<KeyValuePair<AnimationClip, AnimationClip>> overrides, string key, string animationName, int id)
    {
        string resourcePath = $"Animations/Weapon{id}/{animationName}";

        AnimationClip newClip = Resources.Load<AnimationClip>(resourcePath);
        if (newClip != null)
        {
            overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(currentOverrideController[key], newClip));
        }
        else
        {
            Debug.Log($"Failed to load animation from path: {resourcePath}");
        }
    }

    private void SetWeaponAnimations(int switchIdx, int id)
    {
        if (id < 0 || id >= allWeapons.Count)
        {
            Debug.Log("Invalid weapon ID: " + id);
            return;
        }

        if (originalAnimatorController == null)
        {
            Debug.Log("Original Animator Controller is null.");
            return;
        }

        // AnimatorOverrideController를 생성
        if (currentOverrideController == null || currentOverrideController.runtimeAnimatorController != originalAnimatorController)
        {
            // 애니메이션 오버라이드 컨트롤러를 복제
            currentOverrideController = new AnimatorOverrideController(originalAnimatorController);
            animator.runtimeAnimatorController = currentOverrideController;
        }

        // 해당 무기의 애니메이션 세팅
        Animations setWeapon = allWeapons[id].animations;
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new();

        string layerPrefix = $"Slot{switchIdx}";

        // 각 애니메이션을 로드
        AddAnimationOverride(overrides, $"{layerPrefix}Idle", setWeapon.idle, id);
        AddAnimationOverride(overrides, $"{layerPrefix}Combo1", setWeapon.combo1, id);
        AddAnimationOverride(overrides, $"{layerPrefix}Combo2", setWeapon.combo2, id);
        AddAnimationOverride(overrides, $"{layerPrefix}Combo3", setWeapon.combo3, id);
        AddAnimationOverride(overrides, $"{layerPrefix}UpSideAttack", setWeapon.upSideAttack, id);
        AddAnimationOverride(overrides, $"{layerPrefix}DownSideAttack", setWeapon.downSideAttack, id);
        AddAnimationOverride(overrides, $"{layerPrefix}ComboEnd", setWeapon.comboEnd, id);

        // 애니메이션을 덮어쓰기
        foreach (var pair in overrides)
        {
            if (pair.Value != null)
                currentOverrideController[pair.Key] = pair.Value;
        }

        //Debug.Log($"SetWeaponAnimations applied for WeaponSlot{switchIdx} with weapon ID {id}");
    }


    public void ToggleWeaponLayer(int layerIndex, bool activate)
    {
        // 레이어 활성화 여부 설정
        float weight = activate ? 1f : 0f;
        animator.SetLayerWeight(layerIndex, weight);
    }

    public void SwapWeapon()
    {
        (activeWeapons[1], activeWeapons[0]) = (activeWeapons[0], activeWeapons[1]);

        // 두 오브젝트의 Image 컴포넌트의 스프라이트를 서로 교환하려면
        // 두 오브젝트에서 Image 컴포넌트를 가져옴
        Image img1 = activeWeaponObject[0].GetComponent<Image>();
        Image img2 = activeWeaponObject[1].GetComponent<Image>();

        // 임시 변수로 두 스프라이트를 교환
        (img2.sprite, img1.sprite) = (img1.sprite, img2.sprite);

        activeWeaponObject[0].GetComponent<AdjustSpriteSize>().SetSprite();
        activeWeaponObject[1].GetComponent<AdjustSpriteSize>().SetSprite();
        Debug.Log("SwapWeapon 호출");


        if (currentActiveLayer == 1)
        {
            ToggleWeaponLayer(1, false); // WeaponSlot1 레이어를 비활성화
            ToggleWeaponLayer(2, true);  // WeaponSlot2 레이어를 활성화
            Debug.Log("WeaponSlot2 활성화");

            currentActiveLayer = 2;
        }
        else if (currentActiveLayer == 2) {
            ToggleWeaponLayer(1, true);  // WeaponSlot1 레이어를 활성화
            ToggleWeaponLayer(2, false); // WeaponSlot2 레이어를 비활성화
            Debug.Log("WeaponSlot1 활성화");

            currentActiveLayer = 1;
        }
        else
        {
            Debug.Log("Invalid currentActiveLayer value 오류오류올유로유");
        }
    }

    // 아이템 교체 메소드
    public void SwitchActiveItem(int switchIdx, int id)
    {
        //switchIdx = 활성화 무기슬롯, id = 바꿀 무기 아이디
        //if (acquiredWeaponIds.Contains(id))
        //if (acquiredWeaponIds.Contains(id))
        //{
        //    activeWeapons[switchIdx] = id;
        //}
        //else
        //{
        //    return;
        //}

        //이미지 업데이트 
        activeWeapons[switchIdx] = id;
        Weapon wp = WeaponManager.Instance.GetActiveItem(id);
        Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
        activeWeaponObject[switchIdx].GetComponent<Image>().sprite = sprite;
        activeWeaponObject[switchIdx].GetComponent<AdjustSpriteSize>().SetSprite();

        //애니메이션 업데이트
        SetWeaponAnimations(switchIdx+1, id);
    }

    //받은 아이템 추가
    public void AddAcquiredItemIds(int AddItemid)
    {
        if (!acquiredWeaponIds.Contains(AddItemid))
        {
            acquiredWeaponIds.Add(AddItemid);
        }
    }

    //현재 허드에 담고있는 아이템들 내용 출력을 위해 값들 가져옴
    public Weapon GetActiveItem(int slot)
    {
        //if (acquiredItemIds.Contains(slot))
        //{
        //      return acquiredItemIds[slot];
        if( slot == -1 ) return null;

        return allWeapons[slot];
        //}
        //return null;
    }

    public List<Weapon> GetAcquiredWeapons()
    {
        //가지고 있는 무기 배열 반복문 한바퀴 돌려서
        //아이디에 맞는 웨폰값들 리스트에 저장해서 반환

        return allWeapons;
    }

}