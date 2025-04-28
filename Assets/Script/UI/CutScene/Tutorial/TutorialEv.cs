using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class TutorialEv : MonoBehaviour
{
    [SerializeField] private GameObject mission;
    [SerializeField] private BoxCollider2D ev;

    private bool isClear;

    // Update is called once per frame
    void Update()
    {
        if (mission.GetComponent<PipeManager>().isClear)
        {
            if (!isClear)
            {
                isClear = true;
                ev.enabled = true;
                gameObject.SetActive(false);
            }
        }
    }
}
