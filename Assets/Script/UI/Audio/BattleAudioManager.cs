using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource battleAudioSource;

    [Header("Hit audio Resources")]
    [SerializeField] private AudioClip shotAudioClip;
    [SerializeField] private AudioClip emptyGunAudioClip;

    public void OnGunAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (battleAudioSource.isPlaying) return; // 이미 재생 중이면 무시

        if (Ver01_DungeonStatManager.Instance.GetCurrentMagazine() > 0 )
        {
            battleAudioSource.PlayOneShot(shotAudioClip);
        }
        else
        {
            battleAudioSource.PlayOneShot(emptyGunAudioClip);
        }
    }
}