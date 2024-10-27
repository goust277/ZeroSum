using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ItemManager<T, A> : MonoBehaviour where T : class where A : IItemCollection<T>
{
    // 싱글톤 인스턴스
    public static ItemManager<T, A> Instance { get; private set; }

    protected List<T> allItems = new List<T>(); // 모든 아이템 리스트
    public List<int> acquiredItemIds = new List<int>(); // 플레이어가 얻은 아이템 리스트
    public int[] activeItems = new int[2]; // 현재 적용중인 아이템 ID 배열


    protected virtual void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환해도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //각각 필요한 파일 불러와서 모든 아이템에 저장해놓기
    protected void LoadData(string path)
    {
        string basePath = Application.dataPath + "/Resources/Json/";
        string fullPath = basePath + path;

        string jsonData = File.ReadAllText(fullPath);

        // T는 아이템 배열로 변환
        A items = JsonConvert.DeserializeObject<A>(jsonData);

        // items 객체에서 Items라는 프로퍼티를 찾아 그 값을 가져와
        //이 값이 IEnumerable<T> 타입으로 변환 가능한 경우 각 요소를 순회하며 item으로 사용
        foreach (var item in items.Items)
        {
            allItems.Add(item);
        }
    }
    // 아이템 교체 메소드
    public void SwitchActiveItem(int switchIdx, int id)
    {
        //switchIdx = 활성화 무기슬롯, id = 바꿀 무기 아이디
        if (acquiredItemIds.Contains(id))
        {
            activeItems[switchIdx] = id;
        }
    }

    //받은 아이템 추가
    public void AddAcquiredItemIds(int AddItemid)
    {
        if (!acquiredItemIds.Contains(AddItemid)) {
            acquiredItemIds.Add(AddItemid);
        }
    }

    public void SwapWeapon()
    {
        (activeItems[1], activeItems[0]) = (activeItems[0], activeItems[1]);

    }


    //현재 허드에 담고있는 아이템들 내용 출력을 위해 값들 가져옴
    public T GetActiveItem(int slot)
    {
        //if (acquiredItemIds.Contains(slot))
        //{
        //      return acquiredItemIds[slot];
        return allItems[slot];
        //}
        //return null;
    }
}
