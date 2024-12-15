using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponSlot : Qslot
{
    [SerializeField] int slotnum; //허드화면에서 0번, 1번(0번이 큰 그림(활성화중인 무기)

    private int slot; //장착중인 아이템 아이디번호 

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
