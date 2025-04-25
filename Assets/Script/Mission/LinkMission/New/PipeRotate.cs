using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeRotate : MonoBehaviour
{
    [SerializeField] private int rotationStep;

    [SerializeField] private int answer;
    [SerializeField] private int sideWayAnswer;


    [SerializeField] private ChangeLink[] change;
    public bool sideWay = false;

    public bool isAnswer = false;

    public event Action answerUp;
    public event Action answerDown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!sideWay)
        {
            if (rotationStep == answer)
            {
                if (!isAnswer)
                {
                    isAnswer = true;
                    answerUp?.Invoke();
                }
            }
            else if (rotationStep != answer)
            {
                if (isAnswer)
                {
                    isAnswer = false;
                    answerDown?.Invoke();
                }
            }
        }
        else if (sideWay)
        {
            if (rotationStep == answer || rotationStep == sideWayAnswer)
            {
                if (!isAnswer)
                {
                    isAnswer = true;
                    answerUp?.Invoke();
                }
            }
            else if (rotationStep != answer && rotationStep != sideWayAnswer)
            {
                if (isAnswer)
                {
                    isAnswer = false;
                    answerDown?.Invoke();
                }
            }
        }
    }

    public void RotatePipe()
    {
        rotationStep = (rotationStep + 1) % 4;
        transform.Rotate(0, 0, -90);

    }

    public int Answer(int value)
    {
        return value++;
    }
}
