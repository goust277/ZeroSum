using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerTScontroll : MonoBehaviour
{
    [SerializeField] private PlayableDirector pd;
    [SerializeField] private TimelineAsset[] timelineAssets;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if(collision.tag.Equals("Player"))
        {
            pd.Play(timelineAssets[0]);
            gameObject.SetActive(false);
        }
    }



}
