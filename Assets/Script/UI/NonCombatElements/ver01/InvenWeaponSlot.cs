using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using UnityEngine.UI;
public class InvenWeaponSlot : Qslot
{
    public int slotnum;
    private Weapon CurrentWeapon;
    private InventoryController inventoryController; //�κ�����

    
    //private int slot; //�������� ������ ���̵��ȣ 
    [SerializeField] private TextMeshProUGUI EffectTXT;

    public void OnChangedSlotnum(int num, TextMeshProUGUI nameTXT, TextMeshProUGUI desText,
        TextMeshProUGUI effectTXT, GameObject iconImage)
    {
        slotnum = num;
        if (slotnum != -1)
        {
            CurrentWeapon = WeaponManager.Instance.GetActiveItem(slotnum);
        }
        this.nameTXT = nameTXT;
        this.desTXT = desText;
        this.EffectTXT = effectTXT;
        this.InfBG = iconImage;
    }

    public void OnClicked()
    {

        inventoryController ??= FindObjectOfType<InventoryController>() as InventoryController;

        //slot = WeaponManager.Instance.activeWeapons[slotnum];
        //transform.localScale = hoverScale;
        //InfBG.SetActive(true);
        if(slotnum == -1)
        {
            nameTXT.text = "";
            desTXT.text = "";
            InfBG.GetComponent<Image>().sprite = inventoryController.emptySprite; // �̹��� ������Ʈ
            EffectTXT.text = "";
        }
        else
        {
            nameTXT.text = CurrentWeapon.weaponName;
            desTXT.text = CurrentWeapon.weaponDes;

            EffectTXT.text = "";
            EffectTXT.text += "���� : " + string.Join(", ", CurrentWeapon.effects.damage) + "\n";
            EffectTXT.text += "���� : " + CurrentWeapon.effects.attackSpeed + "\n";
            EffectTXT.text += "���� : " + CurrentWeapon.effects.range + "\n";

            //Sprite sprite = Resources.Load<Sprite>(Application.dataPath + CurrentWeapon.weaponIcons);
            Sprite sprite = Resources.Load<Sprite>(CurrentWeapon.weaponIcons);
            if (sprite != null)
            {
                InfBG.GetComponent<Image>().sprite = sprite; // �̹��� ������Ʈ
                InfBG.GetComponent<AdjustSpriteSize>().SetSprite();
            }
            else
            {
                Debug.LogError("Sprite not found at path: " + CurrentWeapon.weaponIcons);
            }
        }

        if (inventoryController != null)
        {
            inventoryController.ChangeSelectedWeapon(slotnum);
        }
        else
        {
            Debug.LogWarning("InvenWeaponSlot : inventoryController is nulllllllllll");
        }

        //InfBG.GetComponent<Image>().sprite = sprite;
        //isSelected = true;
    }
}
