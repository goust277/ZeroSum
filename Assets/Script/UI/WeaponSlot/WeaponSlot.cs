using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponSlot : Qslot
{
    [SerializeField] int slotnum; //���ȭ�鿡�� 0��, 1��(0���� ū �׸�(Ȱ��ȭ���� ����)

    private int slot; //�������� ������ ���̵��ȣ 

    private void Start()
    {
        if (InfBG.activeSelf)
        {
            InfBG.SetActive(false);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        slot = WeaponManager.Instance.activeWeapons[slotnum];
        if( slot == -1)
        {
            return;
        }
        else
        {
            Weapon CurrentWeapon = WeaponManager.Instance.GetActiveItem(slot);

            if (!isSelected)
            {
                transform.localScale = hoverScale;
                InfBG.SetActive(true);

                nameTXT.text = CurrentWeapon.weaponName;
                desTXT.text = CurrentWeapon.weaponDes;

                isSelected = true;
            }
        }
    }
}
