using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAudioMonster : MonoBehaviour
{
    [Header("Monster Audio")]
    public AudioSource moveAudioSource;
    public AudioSource battleAudioSource;

    public AudioClip attackClip;
    public AudioClip damagedClip;
    //�״� �Ҹ��� ���� �ִٸ� ����ٰ� �߰�

    public void PlayMoveSound(float pitch)
    {
        if (moveAudioSource == null || moveAudioSource.isPlaying) return;

        if (moveAudioSource.pitch != pitch)
        {
            moveAudioSource.pitch = pitch;
        }
        moveAudioSource.loop = true;
        moveAudioSource.Play();
    }

    public void StopMoveSound()
    {
        if (moveAudioSource == null || !moveAudioSource.isPlaying) return;
        moveAudioSource.Stop();
    }

    public void PlayAttackSound()
    {
        if (battleAudioSource == null || battleAudioSource.isPlaying || attackClip == null) return;

        battleAudioSource.clip = attackClip;
        battleAudioSource.Play();
    }

    public void PlayDamagedSound()
    {
        if (battleAudioSource == null || damagedClip == null) return;

        battleAudioSource.clip = damagedClip;
        battleAudioSource.Play();
    }

    public void PlayDeathSound()
    {
        if (battleAudioSource == null || damagedClip == null) return;
        //���� Ŭ������ �ٲ����.
        battleAudioSource.clip = damagedClip;
        battleAudioSource.Play();
    }
}
