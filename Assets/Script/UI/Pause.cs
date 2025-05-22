using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI; // ESC 누르면 활성화할 UI (자식 버튼 포함)
    [SerializeField] private GameObject settingsUI; // 설정창 UI
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
            Time.timeScale = 0; // 게임 일시정지
            pauseUI.SetActive(true); // 버튼 활성화
        }
        else
        {
            Time.timeScale = 1; // 게임 재개
            pauseUI.SetActive(false);
            settingsUI.SetActive(false); // 설정창도 같이 닫음
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
