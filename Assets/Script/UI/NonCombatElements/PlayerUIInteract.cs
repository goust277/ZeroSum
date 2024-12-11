using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerUIInteract: MonoBehaviour
{
    private DialogueManager dialogueManager; // DialogueManager ����
    private InventoryController inventoryController; //�κ�����

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
                // �κ��丮�� �������� �ʰ� ��ȭ�� ���۵��� �ʾ��� ���� ��ȭ ����
                dialogueManager.StartConversation(CollisionNPC);
                isInputIgnore = true;
            }

            if (!isInteracting && !isOpenInven)
            {
                // �ƹ��͵� �� �ϰ� �ִ� ������ ���� �Է� ó�� ���
                isInputIgnore = false;
            }

            if (isOpenInven)
            {
                // �κ��丮�� ���� ���� ���� ���� ����
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

        if (context.performed && !isInputIgnore)  // context.started ��� context.performed
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
            isInputIgnore = false; // Ű�� �ö� �� �ٽ� �Է� �����ϵ���
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
