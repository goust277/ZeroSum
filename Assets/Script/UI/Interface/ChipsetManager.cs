using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ChipsetManager : ItemManager<Chipset, ChipsetsArray>
{
    //// �̱��� �ν��Ͻ�
    //public static ChipsetManager Instance { get; private set; }

    //public int[] activeChipsets = new int[3]; // ���� �������� Ĩ�� ID �迭
    //public List<int> acquiredChipsetsIds; // �÷��̾ ���� Ĩ�� ����Ʈ
    //private List<Chipset> allChipsets = new List<Chipset>(); // ��� Ĩ�� ����Ʈ

    protected override void Awake()
    {
        base.Awake(); // �θ� Ŭ������ Awake ȣ��
        LoadData("ChipSets.json"); // Ĩ�� ������ �ε�
    }

    // JSON �����͸� �о ��� Ĩ�� ����Ʈ ����
    //void LoadChipsetData()
    //{
    //    string basePath = Application.dataPath + "/ZeroSum/Resources/Json/";
    //    basePath = basePath + "ChipSets.json";

    //    string jsonData = File.ReadAllText(basePath);

    //    ChipsetsArray chipsets = JsonConvert.DeserializeObject<ChipsetsArray>(jsonData);

    //    foreach (Chipset cs in chipsets.ChipSets)
    //    {
    //        allItems.Add(cs);
    //        //���� Ȯ�� ������
    //        //Debug.Log($"Name: {cs.name}, Description: {cs.des}, Type: {cs.type}");
    //        //foreach (var effect in cs.effect)
    //        //{
    //        //    Debug.Log($"Effect: {effect.Key}, Value: {effect.Value}");
    //        //}
    //    }
    //}
}