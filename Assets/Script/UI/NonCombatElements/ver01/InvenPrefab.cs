using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenPrefab : MonoBehaviour
{
    public Image thisImage;
    private InventoryController inventoryController; //인벤참조

    private void Awake()
    {
        thisImage = GetComponent<Image>();
    }

    public void OnClicked()
    {

        inventoryController ??= FindObjectOfType<InventoryController>() as InventoryController;
        if (inventoryController != null)
        {
            inventoryController.PrefabSelected(thisImage);
        }
        else
        {
            Debug.LogWarning("InvenPrefab : inventoryController is nulllllllllll");
        }
    }

}