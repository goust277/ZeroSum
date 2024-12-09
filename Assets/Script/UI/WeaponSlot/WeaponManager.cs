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

    public int[] activeWeapons = new int[2]; // ���� �������� ������ ID �迭
    private List<int> acquiredWeaponIds; // �÷��̾ ���� ���� ����Ʈ
    private List<Weapon> allWeapons = new List<Weapon>(); // ��� Ĩ�� ����Ʈ

    [SerializeField] private GameObject[] activeWeaponObject = new GameObject[2];


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ�ص� ����
        }
        else
        {
            Destroy(gameObject);
        }

        LoadData("Ver00/Dataset/Weapons.json"); // ���� ������ �ε�

    }

    private void LoadData(string path)
    {
        string basePath = Application.dataPath + "/Resources/Json/";
        string fullPath = basePath + path;

        string jsonData = File.ReadAllText(fullPath);

        // T�� ������ �迭�� ��ȯ
        WeaponsArray items = JsonConvert.DeserializeObject<WeaponsArray>(jsonData);

        // items ��ü���� Items��� ������Ƽ�� ã�� �� ���� ������
        //�� ���� IEnumerable<T> Ÿ������ ��ȯ ������ ��� �� ��Ҹ� ��ȸ�ϸ� item���� ���
        foreach (var item in items.Items)
        {
            allWeapons.Add(item);
        }
    }

    public void SwapWeapon()
    {
        (activeWeapons[1], activeWeapons[0]) = (activeWeapons[0], activeWeapons[1]);

        // �� ������Ʈ�� Image ������Ʈ�� ��������Ʈ�� ���� ��ȯ�Ϸ���
        // �� ������Ʈ���� Image ������Ʈ�� ������
        Image img1 = activeWeaponObject[0].GetComponent<Image>();
        Image img2 = activeWeaponObject[1].GetComponent<Image>();

        // �ӽ� ������ �� ��������Ʈ�� ��ȯ
        (img2.sprite, img1.sprite) = (img1.sprite, img2.sprite);

        activeWeaponObject[0].GetComponent<AdjustSpriteSize>().SetSprite();
        activeWeaponObject[1].GetComponent<AdjustSpriteSize>().SetSprite();

    }

    // ������ ��ü �޼ҵ�
    public void SwitchActiveItem(int switchIdx, int id)
    {
        //switchIdx = Ȱ��ȭ ���⽽��, id = �ٲ� ���� ���̵�
        //if (acquiredWeaponIds.Contains(id))
        //if (acquiredWeaponIds.Contains(id))
        //{
        //    activeWeapons[switchIdx] = id;
        //}
        //else
        //{
        //    return;
        //}

        activeWeapons[switchIdx] = id;

        Weapon wp = WeaponManager.Instance.GetActiveItem(id);
        Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
        activeWeaponObject[switchIdx].GetComponent<Image>().sprite = sprite;// �̹��� ������Ʈ
        activeWeaponObject[switchIdx].GetComponent<AdjustSpriteSize>().SetSprite();

    }

    //���� ������ �߰�
    public void AddAcquiredItemIds(int AddItemid)
    {
        if (!acquiredWeaponIds.Contains(AddItemid))
        {
            acquiredWeaponIds.Add(AddItemid);
        }
    }

    //���� ��忡 ����ִ� �����۵� ���� ����� ���� ���� ������
    public Weapon GetActiveItem(int slot)
    {
        //if (acquiredItemIds.Contains(slot))
        //{
        //      return acquiredItemIds[slot];
        return allWeapons[slot];
        //}
        //return null;
    }

    public List<Weapon> GetAcquiredWeapons()
    {
        //������ �ִ� ���� �迭 �ݺ��� �ѹ��� ������
        //���̵� �´� �������� ����Ʈ�� �����ؼ� ��ȯ

        return allWeapons;
    }

}