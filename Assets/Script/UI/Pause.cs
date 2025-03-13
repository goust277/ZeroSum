using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI; // ESC ������ Ȱ��ȭ�� UI (�ڽ� ��ư ����)
    [SerializeField] private GameObject settingsUI; // ����â UI
    private bool isPaused = false;

    private void Start()
    {
        pauseUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
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
}
