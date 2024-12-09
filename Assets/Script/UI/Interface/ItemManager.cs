using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ItemManager<T, A> : MonoBehaviour where T : class where A : IItemCollection<T>
{
    // �̱��� �ν��Ͻ�
    public static ItemManager<T, A> Instance { get; private set; }

    protected List<T> allItems = new List<T>(); // ��� ������ ����Ʈ
    public List<int> acquiredItemIds = new List<int>(); // �÷��̾ ���� ������ ����Ʈ
    public int[] activeItems = new int[2]; // ���� �������� ������ ID �迭


    protected virtual void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ�ص� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //���� �ʿ��� ���� �ҷ��ͼ� ��� �����ۿ� �����س���
    protected void LoadData(string path)
    {
        string basePath = Application.dataPath + "/Resources/Json/";
        string fullPath = basePath + path;

        string jsonData = File.ReadAllText(fullPath);

        // T�� ������ �迭�� ��ȯ
        A items = JsonConvert.DeserializeObject<A>(jsonData);

        // items ��ü���� Items��� ������Ƽ�� ã�� �� ���� ������
        //�� ���� IEnumerable<T> Ÿ������ ��ȯ ������ ��� �� ��Ҹ� ��ȸ�ϸ� item���� ���
        foreach (var item in items.Items)
        {
            allItems.Add(item);
        }
    }
    // ������ ��ü �޼ҵ�
    public void SwitchActiveItem(int switchIdx, int id)
    {
        //switchIdx = Ȱ��ȭ ���⽽��, id = �ٲ� ���� ���̵�
        if (acquiredItemIds.Contains(id))
        {
            activeItems[switchIdx] = id;
        }
    }

    //���� ������ �߰�
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


    //���� ��忡 ����ִ� �����۵� ���� ����� ���� ���� ������
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
