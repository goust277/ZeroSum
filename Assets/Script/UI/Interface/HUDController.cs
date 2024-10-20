using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.IO;

public class HUDController : MonoBehaviour
{
    [SerializeField] GameObject SlotText;



    void Start()
    {
        SlotText.SetActive(false);
        // WeaponManager와 ChipsetManager 인스턴스 생성 + 데이터 로드
        WeaponManager weaponManager = gameObject.AddComponent<WeaponManager>();
        ChipsetManager chipsetManager = gameObject.AddComponent<ChipsetManager>();

        //weaponManager.activeItems에 적용
        weaponManager.activeItems[0] = 0; // 0번 칩셋
        weaponManager.activeItems[1] = 1; // 2번 칩셋

        //chipsetManager.activeItems에 적용
        chipsetManager.activeItems[0] = 0; // 0번 칩셋
        //chipsetManager.activeItems[1] = 1; // 2번 칩셋
    }
}
