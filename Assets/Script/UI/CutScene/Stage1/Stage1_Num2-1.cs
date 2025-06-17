using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_Num2_1 : CutSceneBase
{
    [SerializeField] private MissionDoor missionDoor;
    [SerializeField] private bool isEntered;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (isEntered && !hasPlayed)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                trigger.enabled = false;
                inputManager.SetActive(false); //입력못받게하고
                hasPlayed = true;
                StartCoroutine(Num2_1Scene());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            isEntered = true;
        }
    }

    private IEnumerator Num2_1Scene()
    {
        yield return new WaitForSeconds(0.5f);

        yield return ShowDialog(0, 5.0f); //유저가 대사하고

        inputManager.SetActive(true);
        missionDoor.Exe();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            isEntered = false;
        }
    }

}
