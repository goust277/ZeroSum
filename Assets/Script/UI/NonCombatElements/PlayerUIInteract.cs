using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerUIInteract: MonoBehaviour
{
    private DialogueManager dialogueManager; // DialogueManager ����
    private InventoryController inventoryController; //�κ�����
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

        if (context.started && !isInputIgnore) // �Է� ���� ���°� �ƴ� ��쿡�� ����
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

        if (context.started && !isInputIgnore)  // ��ȣ�ۿ밡���� �� �κ��丮 ���ݱ�
        {
            Debug.Log("isOpenInven =" + isOpenInven);

            if (!isOpenInven) //�ݰ������� �������
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
