using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ChipsetManager : ItemManager<Chipset, ChipsetsArray>
{
    //// 싱글톤 인스턴스
    //public static ChipsetManager Instance { get; private set; }

    //public int[] activeChipsets = new int[3]; // 현재 적용중인 칩셋 ID 배열
    //public List<int> acquiredChipsetsIds; // 플레이어가 얻은 칩셋 리스트
    //private List<Chipset> allChipsets = new List<Chipset>(); // 모든 칩셋 리스트

    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 호출
        LoadData("ChipSets.json"); // 칩셋 데이터 로드
    }

    // JSON 데이터를 읽어서 모든 칩셋 리스트 갱신
    //void LoadChipsetData()
    //{
    //    string basePath = Application.dataPath + "/ZeroSum/Resources/Json/";
    //    basePath = basePath + "ChipSets.json";

    //    string jsonData = File.ReadAllText(basePath);

    //    ChipsetsArray chipsets = JsonConvert.DeserializeObject<ChipsetsArray>(jsonData);

    //    foreach (Chipset cs in chipsets.ChipSets)
    //    {
    //        allItems.Add(cs);
    //        //내용 확인 디버깅용
    //        //Debug.Log($"Name: {cs.name}, Description: {cs.des}, Type: {cs.type}");
    //        //foreach (var effect in cs.effect)
    //        //{
    //        //    Debug.Log($"Effect: {effect.Key}, Value: {effect.Value}");
    //        //}
    //    }
    //}
}