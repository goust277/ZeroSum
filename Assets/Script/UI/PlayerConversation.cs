using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConversation : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager; // DialogueManager ÂüÁ¶

    public bool isInteracting = false;
    [HideInInspector] public int CollisionNPC = 0;
    void Start()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("PlayerConversation >> DialogueManager is not assigned");
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && isInteracting && !dialogueManager.isConversation)
        {
            dialogueManager.StartConversation(CollisionNPC);
        }
    }

}
