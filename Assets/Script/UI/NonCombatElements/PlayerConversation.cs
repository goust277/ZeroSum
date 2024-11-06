using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConversation : MonoBehaviour
{
    private DialogueManager dialogueManager; // DialogueManager ÂüÁ¶

    public bool isInteracting = false;
    [HideInInspector] public int CollisionNPC = 0;
    void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>() as DialogueManager;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && isInteracting && !dialogueManager.isConversation)
        {
            dialogueManager.StartConversation(CollisionNPC);
        }
    }

    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        WeaponManager.Instance.SwapWeapon();
    }

}
