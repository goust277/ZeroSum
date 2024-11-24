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
    // [0]type 0 List<int> : type 0번인 무기 아이디 값 (총)
    // [1]type 1 List<int> : type 1번인 무기 아이디 값 (활)
    // [2]type 2 List<int> : type 2번인 무기 아이디 값 (검)
    // [3]type 3 List<int> : type 3번인 무기 아이디 값 (둔기)

    [Header("프리팹생성소스")]
    [SerializeField] private TextMeshProUGUI nameTXT;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI effectTXT;
    [SerializeField] private GameObject iconImage;

    [Header("무기스위칭소스")]
    [SerializeField] private GameObject[] activeWeaponObject = new GameObject[2];
    [SerializeField] private GameObject WarningObj;
    //private bool isWarningActive = false; // 경고창 활성화 상태

    public int currentSelectedSlot = 0;
    public int selectedWeapon;
    [SerializeField] private Image selectedWeaponImage;
    private int currentSelectedCategory = -1; // 현재 선택된 카테고리 (-1: 선택 없음)

    [Header("인벤토리 스프라이트소스")]
    [SerializeField] private Sprite selectedSprite; // 선택된 버튼 색상
    [SerializeField] private Sprite defaultSprite;  // 기본 버튼 색상
    [SerializeField] private Sprite selectedSlotSprite; // 선택된 버튼 색상
    [SerializeField] private Sprite defaultSSlotprite;  // 기본 버튼 색상
    [SerializeField] public Sprite emptySprite;  // 기본 버튼 색상



    protected override void Awake()
    {
        base.Awake();
        selectedWeapon = -1;
    }

    protected override void Start()
    {
        WeaponTypeSetting();
        gameObject.SetActive(false);

        activeWeapon[0] = -1;
        activeWeapon[1] = -1;

        WarningObj.SetActive(false);
    }

    public void ChangeSelectedSlot(int slot)
    {
        currentSelectedSlot = slot;
        //Debug.Log("★UN currentSelectedSlot" + Mathf.Abs(slot - 1));

        activeWeaponObject[slot].transform.parent.GetComponent<Image>().sprite = selectedSlotSprite;
        activeWeaponObject[Mathf.Abs(slot - 1)].transform.parent.GetComponent<Image>().sprite = defaultSSlotprite;
    }

    public void ChangeSelectedWeapon(int slot)
    {
        //Debug.Log("☆ChangeSelectedWeapon" + selectedWeapon);
        selectedWeapon = slot;
    }

    public void PrefabSelected(Image image)
    {
        if (selectedWeaponImage != null)
        {
            selectedWeaponImage.sprite = defaultSprite;
        }
        image.sprite = selectedSprite;
        selectedWeaponImage = image;
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
        //같은 버튼 계쏙 누르는거면 반응안하는게 나을것 같다.
        if (optionIndex == currentSelectedCategory) 
        {
            return;
        }

        // 기존 선택된 버튼의 색상 복원
        if (currentSelectedCategory != -1)
        {
            btns[currentSelectedCategory].image.sprite = defaultSprite;
        }

        // 새로운 버튼 색상 변경
        btns[optionIndex].image.sprite = selectedSprite;
        currentSelectedCategory = optionIndex; // 현재 선택된 카테고리 업데이트

        if (selectedWeapon != -1)
        {
            selectedWeaponImage.sprite = defaultSprite;
        }

        UpdateWeaponList(optionIndex);
    }
    private void UpdateWeaponList(int optionIndex)
    {
        if (!weaponsByType.ContainsKey(optionIndex))
        {
            return; // 선택된 카테고리에 무기가 없으면 리턴
        }

        List<int> newItems = weaponsByType[optionIndex];
        int itemCount = newItems.Count;

        // 현재 아이템이 적으면 새로 생성, 많으면 삭제
        if (currentItems.Count < itemCount)
        {
            for (int i = currentItems.Count; i < itemCount; i++)
            {
                GameObject newItem = Instantiate(listItemPrefab, contentTransform);
                currentItems.Add(newItem);
            }
        }
        else if (currentItems.Count > itemCount)
        {
            for (int i = itemCount; i < currentItems.Count; i++)
            {
                Destroy(currentItems[i]);
            }
            currentItems.RemoveRange(itemCount, currentItems.Count - itemCount);
        }

        // 아이템의 내용 업데이트
        for (int i = 0; i < itemCount; i++)
        {
            var item = currentItems[i];
            item.GetComponent<InvenWeaponSlot>().OnChangedSlotnum(newItems[i], nameTXT, desText, effectTXT, iconImage);
            item.GetComponentInChildren<TextMeshProUGUI>().text = WeaponManager.Instance.GetActiveItem(newItems[i]).weaponName;
        }
    }

    private IEnumerator WarningEffect()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
        WarningObj.SetActive(false);
    }


    public void SlotChange()
    {
        int oppo = activeWeapon[Mathf.Abs(currentSelectedSlot - 1)];

        if(selectedWeapon == oppo)
        {
            WarningObj.SetActive(true);
            StartCoroutine(WarningEffect());
            //Debug.Log("이미 장착된 무기");
            return;
        }

        Weapon wp = WeaponManager.Instance.GetActiveItem(selectedWeapon);
        Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
        activeWeaponObject[currentSelectedSlot].GetComponent<Image>().sprite = sprite;// 이미지 업데이트
        activeWeaponObject[currentSelectedSlot].GetComponent<AdjustSpriteSize>().SetSprite();
        activeWeaponObject[currentSelectedSlot].GetComponent<InvenWeaponSlot>().OnChangedSlotnum(selectedWeapon, nameTXT, desText, effectTXT, iconImage);

        activeWeapon[currentSelectedSlot] = selectedWeapon;

        ResetParams();

    }

    public void InventoryOpen() //인벤토리 켰을 떄 액티브 위치 맞춰서 메인/서브 슬롯 배치해줘야 함.
    {

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

    private void ResetParams()
    {
        activeWeaponObject[0].transform.parent.GetComponent<Image>().sprite = defaultSSlotprite;
        activeWeaponObject[1].transform.parent.GetComponent<Image>().sprite = defaultSSlotprite;
        currentSelectedSlot = -1;
        selectedWeapon = -1;

        if(currentSelectedCategory != -1)
        {
            btns[currentSelectedCategory].image.sprite = defaultSprite;
            currentSelectedCategory = -1;
        }
        for (int i = 0; i < currentItems.Count; i++)
        {
            Destroy(currentItems[i]);
        }
        currentItems.Clear();

        nameTXT.text = "";
        desText.text = "";
        iconImage.GetComponent<Image>().sprite = emptySprite; // 이미지 업데이트
        effectTXT.text = "";
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

        activeWeapon[0] = -1;
        activeWeapon[1] = -1;

        ResetParams();
    }

}
