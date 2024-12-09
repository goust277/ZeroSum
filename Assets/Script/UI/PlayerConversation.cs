
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConversation : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager; // DialogueManager ����

    public bool isInteracting = false;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && isInteracting && !dialogueManager.isConversation)
        {
            dialogueManager.StartConversation();
        }
    }
}
