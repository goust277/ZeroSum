using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAudio : MonoBehaviour
{
    [SerializeField] private MoveAudioManager moveAudio;

    private AudioSource walkAudioSource;
    private AudioSource runAudioSource;

    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isRunning = false;

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

    public void PlayRunAudio()
    {
        runAudioSource.volume = 0.35f;
        runAudioSource.PlayOneShot(runAudioSource.clip);
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
            isRunning = false;
            walkAudioSource.Stop();
            runAudioSource.Stop();
        }
    }
}
