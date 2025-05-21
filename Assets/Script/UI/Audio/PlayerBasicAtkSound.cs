using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAtkSound : MonoBehaviour
{
    public AudioSource swordAudioSource;
    public AudioClip swordAttackClip;

    public void PlaySwordAttackSound()
    {
        swordAudioSource.PlayOneShot(swordAttackClip);
    }
}
