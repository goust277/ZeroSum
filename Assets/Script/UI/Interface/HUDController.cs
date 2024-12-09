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
        // WeaponManager�� ChipsetManager �ν��Ͻ� ���� + ������ �ε�
        WeaponManager weaponManager = gameObject.AddComponent<WeaponManager>();
        ChipsetManager chipsetManager = gameObject.AddComponent<ChipsetManager>();

        //weaponManager.activeItems�� ����
        weaponManager.activeItems[0] = 0; // 0�� Ĩ��
        weaponManager.activeItems[1] = 1; // 2�� Ĩ��

        //chipsetManager.activeItems�� ����
        chipsetManager.activeItems[0] = 0; // 0�� Ĩ��
        //chipsetManager.activeItems[1] = 1; // 2�� Ĩ��
    }
}
