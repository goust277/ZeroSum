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

        /*******Debug*******/
        // 각 적용된 무기와 칩셋의 정보를 출력
        //for (int i = 0; i < weaponManager.activeItems.Length; i++)
        //{
        //    int weaponId = weaponManager.activeItems[i];
        //    Debug.Log($"Active Weapon ID: {weaponId}, Name: {weaponManager.GetActiveItem(weaponId).des}");
        //}

        //for (int i = 0; i < chipsetManager.activeItems.Length; i++)
        //{
        //    int chipsetId = chipsetManager.activeItems[i];
        //    Debug.Log($"Active Chipset ID: {chipsetId}, Name: {chipsetManager.GetActiveItem(chipsetId).des}");
        //}
    }
}
