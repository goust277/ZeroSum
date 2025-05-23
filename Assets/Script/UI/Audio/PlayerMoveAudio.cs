using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAudio : MonoBehaviour
{
    [SerializeField] private MoveAudioManager moveAudio;

    private AudioSource walkAudioSource;
    private AudioSource runAudioSource;

    private bool isMoving = false;
    private bool isRunning = false;

    private void Start()
    {
        walkAudioSource = moveAudio.walkAudioSource;
        runAudioSource = moveAudio.runAudioSource;
    }

    public void PlayMoveAudio()
    {
        if (!isMoving)
        {
            isMoving = true;
            walkAudioSource.loop = true;
            walkAudioSource.Play();
        }
    }

    public void StopMoveAudio()
    {
        if (isMoving)
        {
            isMoving = false;
            walkAudioSource.Stop();
        }
    }
}
