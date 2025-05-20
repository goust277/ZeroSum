using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;


public class MoveAudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource moveAudioSource;
    [SerializeField] private AudioClip walkAudioClip;
    [SerializeField] private AudioClip runAudioClip;

    private Vector2 moveInput = Vector2.zero;
    private bool isMoving = false;
    private bool isRunning = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        if (context.performed && dir != Vector2.zero)
        {
            if (!isMoving && !isRunning)
            {
                isMoving = true;
                moveAudioSource.clip = walkAudioClip;
                moveAudioSource.loop = true;
                moveAudioSource.Play();
                Debug.Log("🎵 걷기 소리 시작");
            }
        }
        else if (context.canceled || dir == Vector2.zero)
        {
            if (moveAudioSource.isPlaying)
            {
                isMoving = false;
                moveAudioSource.Stop();
                Debug.Log("🛑 걷기 소리 정지");
            }
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isRunning && moveAudioSource.clip != runAudioClip)
            {
                Debug.Log("MoveAudioManager 0 OnDash on");
                isRunning = true;
                moveAudioSource.clip = runAudioClip;
                moveAudioSource.loop = true;
                moveAudioSource.Play();
            }
        }
        else if (context.canceled)
        {
            Debug.Log("MoveAudioManager 0 OnDash false");
            if (moveInput != Vector2.zero)
            {
                isRunning = false;
                moveAudioSource.clip = walkAudioClip;
                moveAudioSource.loop = true;
                moveAudioSource.Play();
            }
        }
    }
}
