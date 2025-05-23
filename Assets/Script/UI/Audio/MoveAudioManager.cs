using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;


public class MoveAudioManager : MonoBehaviour
{
    public AudioSource walkAudioSource;
    public AudioSource runAudioSource;

    private Vector2 moveInput = Vector2.zero;
    private bool isMoving = false;
    public bool isRunning = false;

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isRunning)
            {
                isRunning = true;
                runAudioSource.loop = true;
                runAudioSource.Play();
            }
        }
        else if (context.canceled)
        {
            if (isRunning)
            {
                isMoving = false;
                isRunning = false;
                runAudioSource.Stop();
            }
        }
    }

}
