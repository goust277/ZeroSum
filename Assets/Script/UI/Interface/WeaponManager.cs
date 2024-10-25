using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class WeaponManager : ItemManager<Weapon, WeaponsArray>
{
    //public static WeaponManager Instance { get; private set; }

    //public int[] activeWeapons = new int[3]; // 현재 적용중인 무기의 ID 배열
    //public List<int> acquiredWeaponIds; // 플레이어가 얻은 무기 리스트
    //private List<Weapon> allWeapons = new List<Weapon>(); // 모든 칩셋 리스트

    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 호출
        LoadData("Ver00/Dataset/Weapons.json"); // 무기 데이터 로드
        if( allItems.Count == 0)
        {
            Debug.Log("Fail to read JSON");
        }
    }

    //// JSON 데이터를 읽어서 모든 칩셋 리스트 갱신
    //void LoadWeaponData()
    //{
    //    string basePath = Application.dataPath + "/ZeroSum/Resources/Json/";
    //    basePath = basePath + "Weapons.json";

    //    string jsonData = File.ReadAllText(basePath);

    //    //Newtonsoft.Json을 사용하여 ChipsetList로 변환
    //    WeaponsArray weapons = JsonConvert.DeserializeObject<WeaponsArray>(jsonData);

    //    foreach (Weapon wp in weapons.Weapons)
    //    {
    //        allItems.Add(wp);
    //    }
    //}

}