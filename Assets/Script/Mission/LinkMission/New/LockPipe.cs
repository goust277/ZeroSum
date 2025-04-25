using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPipe : MonoBehaviour
{
    [SerializeField] private PipeRotate[] pipes;
    [SerializeField] private ChangeLink[] changeLinks;

    private int pipeClear = 0;

    private bool isChange = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pipeClear == pipes.Length)
        {
            if (!isChange) 
            {
                isChange = true;

            }
        }
        else if (pipeClear != pipes.Length) 
        {
            if (isChange)
            {
                isChange = false;

            }
        }
    }

    private void UpAnwerPipe()
    {
        pipeClear++;
    }

    private void DownAnwerPipe()
    {
        pipeClear--;
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
