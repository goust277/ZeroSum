using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class WeaponManager : ItemManager<Weapon, WeaponsArray>
{
    //public static WeaponManager Instance { get; private set; }

    //public int[] activeWeapons = new int[3]; // ���� �������� ������ ID �迭
    //public List<int> acquiredWeaponIds; // �÷��̾ ���� ���� ����Ʈ
    //private List<Weapon> allWeapons = new List<Weapon>(); // ��� Ĩ�� ����Ʈ

    protected override void Awake()
    {
        base.Awake(); // �θ� Ŭ������ Awake ȣ��
        LoadData("Ver00/Dataset/Weapons.json"); // ���� ������ �ε�
        if( allItems.Count == 0)
        {
            Debug.Log("Fail to read JSON");
        }
    }

    //// JSON �����͸� �о ��� Ĩ�� ����Ʈ ����
    //void LoadWeaponData()
    //{
    //    string basePath = Application.dataPath + "/ZeroSum/Resources/Json/";
    //    basePath = basePath + "Weapons.json";

    //    string jsonData = File.ReadAllText(basePath);

    //    //Newtonsoft.Json�� ����Ͽ� ChipsetList�� ��ȯ
    //    WeaponsArray weapons = JsonConvert.DeserializeObject<WeaponsArray>(jsonData);

    //    foreach (Weapon wp in weapons.Weapons)
    //    {
    //        allItems.Add(wp);
    //    }
    //}

}