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
    // type�� ���� ���� �з�
    // type 0 List<int> : type 0���� ���� ���̵� �� (��)
    // type 1 List<int> : type 1���� ���� ���̵� �� (Ȱ)
    // type 2 List<int> : type 2���� ���� ���̵� �� (��)
    // type 3 List<int> : type 3���� ���� ���̵� �� (�б�)

    [Header("�����ջ����ҽ�")]
    [SerializeField] private TextMeshProUGUI nameTXT;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI effectTXT;
    [SerializeField] private GameObject iconImage;

    [Header("���⽺��Ī�ҽ�")]
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
        foreach (var weapon in WeaponManager.Instance.GetAcquiredWeapons()) //���߿� ������ �ִ� ���⸸...,,, ���͸� �ؾ� �ҵ�
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
        // ���� ����Ʈ ������Ʈ
        List<int> newItems = weaponsByType[optionIndex];

        int itemCount = newItems.Count;

        // ���� �������� ������ ���� ����, ������ ����
        if (currentItems.Count < itemCount)
        {
            // ���ο� ������ �߰�
            for (int i = currentItems.Count; i < itemCount; i++)
            {
                GameObject newItem = Instantiate(listItemPrefab, contentTransform);
                currentItems.Add(newItem);
            }
        }
        else if (currentItems.Count > itemCount)
        {
            // ���� ������ ����
            for (int i = itemCount; i < currentItems.Count; i++)
            {
                Destroy(currentItems[i]);
            }
            currentItems.RemoveRange(itemCount, currentItems.Count - itemCount);
        }

        // �������� ���븸 ������Ʈ
        for (int i = 0; i < itemCount; i++)
        {
            var item = currentItems[i];
            // item ������ ����
            item.GetComponent<InvenWeaponSlot>().OnChangedSlotnum(newItems[i], nameTXT, desText, effectTXT, iconImage);
            item.GetComponentInChildren<TextMeshProUGUI>().text = WeaponManager.Instance.GetActiveItem(newItems[i]).weaponName;
        }
    }

    public void SlotChange()
    {
        Weapon wp = WeaponManager.Instance.GetActiveItem(selectedWeapon);
        Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
        activeWeaponObject[currentSelectedSlot].GetComponent<Image>().sprite = sprite;// �̹��� ������Ʈ
        activeWeaponObject[currentSelectedSlot].GetComponent<AdjustSpriteSize>().SetSprite();

        activeWeapon[currentSelectedSlot] = selectedWeapon;
    }

    public void InventoryOpen() //�κ��丮 ���� �� ��Ƽ�� ��ġ ���缭 ����/���� ���� ��ġ����� ��.
    {
        //Debug.Log("WeaponManager.Instance.activeWeapons[0] : " + WeaponManager.Instance.activeWeapons[0]);
        //Debug.Log("WeaponManager.Instance.activeWeapons[1] : " + WeaponManager.Instance.activeWeapons[1]);

        int weaponIndx = WeaponManager.Instance.activeWeapons[0];

        if (weaponIndx != -1)
        {
            Weapon wp = WeaponManager.Instance.GetActiveItem(weaponIndx);
            Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
            activeWeaponObject[0].GetComponent<Image>().sprite = sprite;// �̹��� ������Ʈ
            activeWeaponObject[0].GetComponent<AdjustSpriteSize>().SetSprite();
        }
        activeWeaponObject[0].GetComponent<InvenWeaponSlot>().OnChangedSlotnum(weaponIndx, nameTXT, desText, effectTXT, iconImage);

        weaponIndx = WeaponManager.Instance.activeWeapons[1];
        if (weaponIndx != -1)
        {
            
            Weapon wp = WeaponManager.Instance.GetActiveItem(weaponIndx);
            Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
            activeWeaponObject[1].GetComponent<Image>().sprite = sprite;// �̹��� ������Ʈ
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
