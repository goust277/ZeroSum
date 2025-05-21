using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTracker : MonoBehaviour
{
    void Update()
    {
        foreach (var source in FindObjectsOfType<AudioSource>())
        {
            if (source.isPlaying)
            {
                Debug.Log($"🎧 AudioSource 재생 중: {source.gameObject.name}, Clip: {source.clip?.name}");
            }
        }
    }
}