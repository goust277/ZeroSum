using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI; // ESC ������ Ȱ��ȭ�� UI (�ڽ� ��ư ����)
    [SerializeField] private GameObject settingsUI; // ����â UI
    private bool isPaused = false;
    
    private GameObject playerInput;
    private void Start()
    {
        pauseUI.SetActive(false);

        playerInput = GameObject.Find("InputManager");

    }

    public void OnESC(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        TogglePause();
    }


    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // ���� �Ͻ�����
            pauseUI.SetActive(true); // ��ư Ȱ��ȭ
        }
        else
        {
            Time.timeScale = 1; // ���� �簳
            pauseUI.SetActive(false);
            settingsUI.SetActive(false); // ����â�� ���� ����
        }
    }
    void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.GetComponent<PlayerInput>().actions["ESC"].performed -= OnESC;
        }
    }
}
