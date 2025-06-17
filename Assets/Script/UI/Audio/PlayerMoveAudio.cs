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
        walkAudioSource.volume = 0.35f;
        walkAudioSource.PlayOneShot(walkAudioSource.clip);
    }

    public void PlayDownMoveAudio()
    {
        walkAudioSource.volume = 0.1f;
        walkAudioSource.PlayOneShot(walkAudioSource.clip);
    }

    private void OnDisable()
    {
        StopMoveAudio();
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
