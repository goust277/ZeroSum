using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerTScontroll : MonoBehaviour
{
    [SerializeField] private PlayableDirector pd;
    [SerializeField] private TimelineAsset[] timelineAssets;
    //public ProCamera2D proCamera;

    void Start()
    {
        pd.stopped += OnTimelineStopped;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.name);
        if(collision.tag.Equals("Player"))
        {
            pd.Play(timelineAssets[0]);
            gameObject.SetActive(false);
        }
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        ProCamera2D cam = Camera.main.GetComponent<ProCamera2D>();
        ProCamera2DNumericBoundaries boundaries = Camera.main.GetComponent<ProCamera2DNumericBoundaries>();

        if (cam != null)
        {
            cam.enabled = true;
        }

        if (boundaries != null)
        {
            boundaries.enabled = true;
        }
    }


}
