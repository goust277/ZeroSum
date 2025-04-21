using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [SerializeField] private PipeRotate[] pipes;
    [SerializeField] private ChangeLink[] changeLinks;
    [SerializeField] private GameObject mission;
    [SerializeField] private GameObject player;
    
    private int answerPipe = 0;

    public bool isClear;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (answerPipe == pipes.Length )
        {
            if (!isClear)
            {
                isClear = true;

                ClearMission();
               
                Debug.Log("Å¬¸®¾î");
            }
        }
        else if (answerPipe != pipes.Length)
        {
            if (isClear)
            {
                isClear = false;
            }
        }
    }

    private void ClearMission()
    {
        foreach (var change in changeLinks)
        {
            change.ChangeColor();
        }

        mission.SetActive(false);
        player.SetActive(true);
    }

    public void OnMission()
    {
        mission.SetActive ( true );
        player.SetActive(false);
    }

    private void UpAnwerPipe()
    {
        answerPipe++;
    }

    private void DownAnwerPipe()
    { 
        answerPipe--;
    }

    private void OnEnable()
    {
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].answerUp += UpAnwerPipe;
            pipes[i].answerDown += DownAnwerPipe;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].answerUp -= UpAnwerPipe;
            pipes[i].answerDown -= DownAnwerPipe;
        }
    }
}
