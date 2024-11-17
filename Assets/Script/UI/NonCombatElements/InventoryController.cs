using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class InventoryController : BaseUi
{
    public GameObject listItemPrefab;


    [SerializeField] private Transform contentTransform;

    [SerializeField] private int[] activeWeapon = new int[2];
    private List<GameObject> currentItems = new List<GameObject>();
    private Dictionary<int, List<int>> weaponsByType = new Dictionary<int, List<int>>(); 
    // type에 따른 무기 분류
    // type 0 List<int> : type 0번인 무기 아이디 값 (총)
    // type 1 List<int> : type 1번인 무기 아이디 값 (활)
    // type 2 List<int> : type 2번인 무기 아이디 값 (검)
    // type 3 List<int> : type 3번인 무기 아이디 값 (둔기)

    [Header("프리팹생성소스")]
    [SerializeField] private TextMeshProUGUI nameTXT;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI effectTXT;
    [SerializeField] private GameObject iconImage;

    [Header("무기스위칭소스")]
    [SerializeField] private GameObject[] activeWeaponObject = new GameObject[2];
    public int currentSelectedSlot = 0;
    public int selectedWeapon = -1;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        WeaponTypeSetting();
        gameObject.SetActive(false);
    }

    public void ChangeSelectedSlot(int slot)
    {
        currentSelectedSlot = slot;
        //Debug.Log("currentSelectedSlot" + currentSelectedSlot);
    }

    public void ChangeSelectedWeapon(int slot)
    {
        selectedWeapon = slot;
        //Debug.Log("ChangeSelectedWeapon" + selectedWeapon);
    }

    protected override void ButtonFuncion(string btnName)
    {
        //Debug.Log("Button clicked: " + btnName);

        base.ButtonFuncion(btnName);
        switch (btnName)
        {
            case "Category-Gun":
                //Debug.Log("Gun");
                OnOptionSelected(0);
                break;
            case "Category-Bow":
                //Debug.Log("Bow");
                OnOptionSelected(1);
                break;
            case "Category-Sword":
                //Debug.Log("Sword");
                OnOptionSelected(2);
                break;
            case "Category-Blunt":
                //Debug.Log("Blunt");
                OnOptionSelected(3);
                break;
        }
    }

    private void WeaponTypeSetting()
    {
        foreach (var weapon in WeaponManager.Instance.GetAcquiredWeapons()) //나중에 가지고 있는 무기만...,,, 필터링 해야 할듯
        {
            if (!weaponsByType.ContainsKey(weapon.type))
            {
                weaponsByType[weapon.type] = new List<int>();
            }
            weaponsByType[weapon.type].Add(weapon.weaponId);
        }
    }
    private void OnOptionSelected(int optionIndex)
    {
        // 기존 리스트 업데이트
        List<int> newItems = weaponsByType[optionIndex];

        int itemCount = newItems.Count;

        // 현재 아이템이 적으면 새로 생성, 많으면 삭제
        if (currentItems.Count < itemCount)
        {
            // 새로운 아이템 추가
            for (int i = currentItems.Count; i < itemCount; i++)
            {
                GameObject newItem = Instantiate(listItemPrefab, contentTransform);
                currentItems.Add(newItem);
            }
        }
        else if (currentItems.Count > itemCount)
        {
            // 기존 아이템 제거
            for (int i = itemCount; i < currentItems.Count; i++)
            {
                Destroy(currentItems[i]);
            }
            currentItems.RemoveRange(itemCount, currentItems.Count - itemCount);
        }

        // 아이템의 내용만 업데이트
        for (int i = 0; i < itemCount; i++)
        {
            var item = currentItems[i];
            // item 데이터 갱신
            item.GetComponent<InvenWeaponSlot>().OnChangedSlotnum(newItems[i], nameTXT, desText, effectTXT, iconImage);
            item.GetComponentInChildren<TextMeshProUGUI>().text = WeaponManager.Instance.GetActiveItem(newItems[i]).weaponName;
        }
    }

    public void SlotChange()
    {
        Weapon wp = WeaponManager.Instance.GetActiveItem(selectedWeapon);
        Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
        activeWeaponObject[currentSelectedSlot].GetComponent<Image>().sprite = sprite;// 이미지 업데이트
        activeWeaponObject[currentSelectedSlot].GetComponent<AdjustSpriteSize>().SetSprite();

        activeWeapon[currentSelectedSlot] = selectedWeapon;
    }

    public void InventoryOpen() //인벤토리 켰을 떄 액티브 위치 맞춰서 메인/서브 슬롯 배치해줘야 함.
    {
        //Debug.Log("WeaponManager.Instance.activeWeapons[0] : " + WeaponManager.Instance.activeWeapons[0]);
        //Debug.Log("WeaponManager.Instance.activeWeapons[1] : " + WeaponManager.Instance.activeWeapons[1]);

        int weaponIndx = WeaponManager.Instance.activeWeapons[0];

        if (weaponIndx != -1)
        {
            Weapon wp = WeaponManager.Instance.GetActiveItem(weaponIndx);
            Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
            activeWeaponObject[0].GetComponent<Image>().sprite = sprite;// 이미지 업데이트
            activeWeaponObject[0].GetComponent<AdjustSpriteSize>().SetSprite();
        }
        activeWeaponObject[0].GetComponent<InvenWeaponSlot>().OnChangedSlotnum(weaponIndx, nameTXT, desText, effectTXT, iconImage);

        weaponIndx = WeaponManager.Instance.activeWeapons[1];
        if (weaponIndx != -1)
        {
            
            Weapon wp = WeaponManager.Instance.GetActiveItem(weaponIndx);
            Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
            activeWeaponObject[1].GetComponent<Image>().sprite = sprite;// 이미지 업데이트
            activeWeaponObject[1].GetComponent<AdjustSpriteSize>().SetSprite();
        }
        activeWeaponObject[1].GetComponent<InvenWeaponSlot>().OnChangedSlotnum(weaponIndx, nameTXT, desText, effectTXT, iconImage);
    }

    public void InventoryClose()
    {
        if(activeWeapon[0] != -1)
        {
            WeaponManager.Instance.SwitchActiveItem(0, activeWeapon[0]);
        }

        if (activeWeapon[1] != -1)
        {
            WeaponManager.Instance.SwitchActiveItem(1, activeWeapon[1]);
        }
    }

}
