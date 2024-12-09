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
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>() as DialogueManager;
        }

        if(inventoryController == null)
        {
            inventoryController = FindObjectsOfType<InventoryController>(true).FirstOrDefault();
        }
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
                WeaponManager.Instance.activeWeapons[inventoryController.currentSelectedSlot] = inventoryController.selectedWeapon;
            }
        }
    }

    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (!isInputIgnore)
        {
            WeaponManager.Instance.SwapWeapon();
        }
    }

    public void OnTabInventory(InputAction.CallbackContext context)
    {
        if (context.performed && !isInputIgnore)  // context.started ��� context.performed
        {
            if (!isOpenInven)
            {
                Debug.Log("Inven Open");
                inventoryController.gameObject.SetActive(true);
                inventoryController.InventoryOpen();
                isOpenInven = true;
            }
            else
            {
                Debug.Log("Inven Close");
                inventoryController.gameObject.SetActive(false);
                inventoryController.InventoryClose();
                isOpenInven = false;
            }
            isInputIgnore = true;
        }

        if (context.canceled)
        {
            isInputIgnore = false; // Ű�� �ö� �� �ٽ� �Է� �����ϵ���
        }
    }
}
