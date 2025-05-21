using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseItem : MonoBehaviour
{
    protected AudioSource externalAudioSource;

    protected virtual void Awake()
    {
        if (externalAudioSource == null)
        {
            GameObject audioManager = GameObject.Find("AudioManager");
            if (audioManager == null)
            {
                Debug.LogWarning("AudioManager 오브젝트 xxxx");
            }

            Transform itemChild = audioManager.transform.Find("Item");
            externalAudioSource = itemChild.GetComponent<AudioSource>();
        }
    }

    // 필요 시 자식이 override할 수 있게
    protected void PlaySound()
    {
        if (externalAudioSource != null && externalAudioSource.clip != null)
        {
            externalAudioSource.PlayOneShot(externalAudioSource.clip);
        }
    }
}
