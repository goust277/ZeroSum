using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;


public class MoveAudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource walkAudioSource;
    [SerializeField] private AudioSource runAudioSource;

    private Vector2 moveInput = Vector2.zero;
    private bool isMoving = false;
    private bool isRunning = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        if (context.performed && dir != Vector2.zero)
        {
            if (!isMoving)
            {
                isMoving = true;
                walkAudioSource.loop = true;
                walkAudioSource.Play();
                Debug.Log(" 걷기 소리 시작");
            }
        }
        else if (context.canceled || dir == Vector2.zero)
        {
            if (walkAudioSource.isPlaying)
            {
                isMoving = false;
                walkAudioSource.Stop();
                Debug.Log(" 걷기 소리 정지");

                if (isRunning)
                {
                    runAudioSource.Stop();
                }
            }
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isRunning)
            {
                Debug.Log("MoveAudioManager 0 OnDash on");
                isRunning = true;
                runAudioSource.loop = true;
                runAudioSource.Play();
            }
        }
        else if (context.canceled)
        {
            if (runAudioSource.isPlaying)
            {
                isMoving = false;
                runAudioSource.Stop();
                Debug.Log("MoveAudioManager 0 OnDash off");
            }
        }
    }
}
