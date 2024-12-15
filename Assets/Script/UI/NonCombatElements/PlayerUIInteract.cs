using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerUIInteract: MonoBehaviour
{
    private DialogueManager dialogueManager; // DialogueManager 참조
    private InventoryController inventoryController; //인벤참조

    public bool isInteracting = false;
    public bool isOpenInven = false;
    private bool isInputIgnore = false;
    [HideInInspector] public int CollisionNPC = 0;

    void Start()
    {
        dialogueManager ??= FindObjectsOfType<DialogueManager>(true).FirstOrDefault();

        inventoryController ??= FindObjectsOfType<InventoryController>(true).FirstOrDefault();

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            //Debug.Log("isOpenInven ="+ isOpenInven);
            //Debug.Log("isInteracting =" + isInteracting);
            //Debug.Log("dialogueManager.isConversation =" + dialogueManager.isConversation);
            if (!isOpenInven && isInteracting && !dialogueManager.isConversation)
            {
                // 인벤토리가 열려있지 않고 대화가 시작되지 않았을 때만 대화 시작
                dialogueManager.StartConversation(CollisionNPC);
                isInputIgnore = true;
            }

            if (!isInteracting && !isOpenInven)
            {
                // 아무것도 안 하고 있는 상태일 때만 입력 처리 허용
                isInputIgnore = false;
            }

            if (isOpenInven)
            {
                // 인벤토리가 열려 있을 때만 무기 변경
                isInputIgnore = true;
                inventoryController.SlotChange();
            }
        }
    }

    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (context.started && !isInputIgnore)
        {
            WeaponManager.Instance.SwapWeapon();
        }
    }

    public void OnTabInventory(InputAction.CallbackContext context)
    {
        Debug.Log("isOpenInven =" + isOpenInven);

        if (context.performed && !isInputIgnore)  // context.started 대신 context.performed
        {
            isInputIgnore = true;

            if (!isOpenInven)
            {
                //Debug.Log("Inven Open");
                inventoryController.gameObject.SetActive(true);
                inventoryController.InventoryOpen();
                isOpenInven = true;
                return;
            }


            if (isOpenInven)
            {
                //Debug.Log("Inven Close");
                inventoryController.gameObject.SetActive(false);
                inventoryController.InventoryClose();
                isOpenInven = false;
            }
        }

        if (context.canceled)
        {
            isInputIgnore = false; // 키가 올라갈 때 다시 입력 가능하도록
        }
    }

    public void OnEscEntered(InputAction.CallbackContext context)
    {

        if (context.started && isInputIgnore)
        {
            //SettingsManager.Instance.SettingOnOff();
            isInputIgnore = false;
        }

        if (context.started && !isOpenInven)
        {
            //SettingsManager.Instance.SettingOnOff();
            isInputIgnore = true;
        }
    }
}
