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
    private List<int> acquiredWeaponIds; // 플레이어가 얻은 무기 리스트
    private List<Weapon> allWeapons = new List<Weapon>(); // 모든 칩셋 리스트

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

    private void LoadData(string path)
    {
        string basePath = Application.dataPath + "/Resources/Json/";
        string fullPath = basePath + path;

        string jsonData = File.ReadAllText(fullPath);

        // T는 아이템 배열로 변환
        WeaponsArray items = JsonConvert.DeserializeObject<WeaponsArray>(jsonData);

        // items 객체에서 Items라는 프로퍼티를 찾아 그 값을 가져와
        //이 값이 IEnumerable<T> 타입으로 변환 가능한 경우 각 요소를 순회하며 item으로 사용
        foreach (var item in items.Items)
        {
            allWeapons.Add(item);
        }
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

        activeWeapons[switchIdx] = id;

        Weapon wp = WeaponManager.Instance.GetActiveItem(id);
        Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
        activeWeaponObject[switchIdx].GetComponent<Image>().sprite = sprite;// 이미지 업데이트
        activeWeaponObject[switchIdx].GetComponent<AdjustSpriteSize>().SetSprite();

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