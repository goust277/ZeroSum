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
    // [0]type 0 List<int> : type 0���� ���� ���̵� �� (��)
    // [1]type 1 List<int> : type 1���� ���� ���̵� �� (Ȱ)
    // [2]type 2 List<int> : type 2���� ���� ���̵� �� (��)
    // [3]type 3 List<int> : type 3���� ���� ���̵� �� (�б�)

    [Header("�����ջ����ҽ�")]
    [SerializeField] private TextMeshProUGUI nameTXT;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI effectTXT;
    [SerializeField] private GameObject iconImage;

    [Header("���⽺��Ī�ҽ�")]
    [SerializeField] private GameObject[] activeWeaponObject = new GameObject[2];
    [SerializeField] private GameObject WarningObj;
    //private bool isWarningActive = false; // ���â Ȱ��ȭ ����

    public int currentSelectedSlot = 0;
    public int selectedWeapon;
    [SerializeField] private Image selectedWeaponImage;
    private int currentSelectedCategory = -1; // ���� ���õ� ī�װ� (-1: ���� ����)

    [Header("�κ��丮 ��������Ʈ�ҽ�")]
    [SerializeField] private Sprite selectedSprite; // ���õ� ��ư ����
    [SerializeField] private Sprite defaultSprite;  // �⺻ ��ư ����
    [SerializeField] private Sprite selectedSlotSprite; // ���õ� ��ư ����
    [SerializeField] private Sprite defaultSSlotprite;  // �⺻ ��ư ����
    [SerializeField] public Sprite emptySprite;  // �⺻ ��ư ����



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
        //Debug.Log("��UN currentSelectedSlot" + Mathf.Abs(slot - 1));

        activeWeaponObject[slot].transform.parent.GetComponent<Image>().sprite = selectedSlotSprite;
        activeWeaponObject[Mathf.Abs(slot - 1)].transform.parent.GetComponent<Image>().sprite = defaultSSlotprite;
    }

    public void ChangeSelectedWeapon(int slot)
    {
        //Debug.Log("��ChangeSelectedWeapon" + selectedWeapon);
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
        //���� ��ư ��� �����°Ÿ� �������ϴ°� ������ ����.
        if (optionIndex == currentSelectedCategory) 
        {
            return;
        }

        // ���� ���õ� ��ư�� ���� ����
        if (currentSelectedCategory != -1)
        {
            btns[currentSelectedCategory].image.sprite = defaultSprite;
        }

        // ���ο� ��ư ���� ����
        btns[optionIndex].image.sprite = selectedSprite;
        currentSelectedCategory = optionIndex; // ���� ���õ� ī�װ� ������Ʈ

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
            return; // ���õ� ī�װ��� ���Ⱑ ������ ����
        }

        List<int> newItems = weaponsByType[optionIndex];
        int itemCount = newItems.Count;

        // ���� �������� ������ ���� ����, ������ ����
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

        // �������� ���� ������Ʈ
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
            //Debug.Log("�̹� ������ ����");
            return;
        }

        Weapon wp = WeaponManager.Instance.GetActiveItem(selectedWeapon);
        Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
        activeWeaponObject[currentSelectedSlot].GetComponent<Image>().sprite = sprite;// �̹��� ������Ʈ
        activeWeaponObject[currentSelectedSlot].GetComponent<AdjustSpriteSize>().SetSprite();
        activeWeaponObject[currentSelectedSlot].GetComponent<InvenWeaponSlot>().OnChangedSlotnum(selectedWeapon, nameTXT, desText, effectTXT, iconImage);

        activeWeapon[currentSelectedSlot] = selectedWeapon;

        ResetParams();

    }

    public void InventoryOpen() //�κ��丮 ���� �� ��Ƽ�� ��ġ ���缭 ����/���� ���� ��ġ����� ��.
    {

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
        iconImage.GetComponent<Image>().sprite = emptySprite; // �̹��� ������Ʈ
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
