using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponSlot : Qslot
{
    [SerializeField] int slotnum;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        var weaponManager = WeaponManager.Instance;

        if (!isSelected)
        {
            transform.localScale = hoverScale;
            InfBG.SetActive(true);

            nameTXT.text = weaponManager.GetActiveItem(slotnum).name;
            desTXT.text = weaponManager.GetActiveItem(slotnum).des;

            isSelected = true;
        }
    }
}
