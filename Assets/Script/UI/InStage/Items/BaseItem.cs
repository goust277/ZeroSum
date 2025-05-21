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
                Debug.LogWarning("AudioManager ������Ʈ xxxx");
            }

            Transform itemChild = audioManager.transform.Find("Item");
            externalAudioSource = itemChild.GetComponent<AudioSource>();
        }
    }

    // �ʿ� �� �ڽ��� override�� �� �ְ�
    protected void PlaySound()
    {
        if (externalAudioSource != null && externalAudioSource.clip != null)
        {
            externalAudioSource.PlayOneShot(externalAudioSource.clip);
        }
    }
}
