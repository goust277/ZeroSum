using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerUIInteract: MonoBehaviour
{
    private DialogueManager dialogueManager; // DialogueManager 참조
    private InventoryController inventoryController; //인벤참조
    private SettingsManager settingsManager;


    public bool isInteracting = false;
    public bool isOpenInven = false;
    private bool isInputIgnore = false;
    private bool isOpenOption = false;

    [HideInInspector] public int CollisionNPC = 0;

    void Start()
    {
        dialogueManager ??= FindObjectsOfType<DialogueManager>(true).FirstOrDefault();
        settingsManager ??= FindObjectsOfType<SettingsManager>(true).FirstOrDefault();
        inventoryController ??= FindObjectsOfType<InventoryController>(true).FirstOrDefault();

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

        if (context.started && !isInputIgnore) // 입력 무시 상태가 아닌 경우에만 실행
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
                //isInputIgnore = true;
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

        if (context.started && !isInputIgnore)  // 상호작용가능할 때 인벤토리 여닫기
        {
            Debug.Log("isOpenInven =" + isOpenInven);

            if (!isOpenInven) //닫겨있으면 열어야지
            {
                //Debug.Log("Inven Open");
                inventoryController.gameObject.SetActive(true);
                inventoryController.InventoryOpen();
                isOpenInven = true;
                //isInputIgnore = true;
                return;
            }


            if (isOpenInven)
            {
                //Debug.Log("Inven Close");
                inventoryController.gameObject.SetActive(false);
                inventoryController.InventoryClose();
                isOpenInven = false;
                isInputIgnore = false;
            }
        }
    }

    public void OnEscEntered(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            settingsManager.SettingOnOff();

            if (!isOpenOption)
            {
                isOpenOption = true;
                isInputIgnore = true;
            }
            else
            {
                isOpenOption = false;         
                isInputIgnore = false;
            }
        }
    }
}
