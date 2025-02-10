using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChipsetSlot : Qslot
{
    [SerializeField] int slotnum;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        var chipsetManager = ChipsetManager.Instance;

        if (!isSelected)
        {
            transform.localScale = hoverScale;
            InfBG.SetActive(true);

            nameTXT.text = chipsetManager.GetActiveItem(slotnum).name;
            desTXT.text = chipsetManager.GetActiveItem(slotnum).des;

            isSelected = true;
        }
    }
}
