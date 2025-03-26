using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ControlEv : BaseEv, IControllable
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float waitingTime;
    [SerializeField] private float moveCount;
    [SerializeField] private bool isUp;

    [Header("ButtonTransform")]
    [SerializeField] private Transform[] buttonTransforms;

    [HideInInspector] public int btnNum;

    private float curMoveCount;
    private float curWaitTime;
    private bool isMoving = false;
    private bool isBtnOn;

    private Vector3 targetPosition;
    private Vector3 topPosition;
    void Start()
    {
        targetPosition = transform.position;
        curWaitTime = 0.0f;
        curMoveCount = 0f;
        isBottom = false;
        isBtnOn = false;
        if (movePosition.y > 0)
        {
            topPosition = transform.position + (movePosition * moveCount);
        }
        else if (movePosition.y < 0)
        {
            topPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curMoveCount == moveCount)
        {
            if (transform.position.y - targetPosition.y > 0f)
            {
                if (!isBottom)
                {
                    isBottom = true;
                }
            }
            else
            {
                if (isBottom)
                {
                    isBottom = false;
                }
            }
        }
        else
        {
            if (isBottom)
            {
                isBottom = false;
            }
        }

    }
    private void FixedUpdate()
    {
        if (!isMoving)
            MoveToTarget();
        else
            MovingToTarget();
    }
    private void MoveToTarget()
    {
        if (curMoveCount == moveCount)
        {
            curMoveCount = 0f;
            if(isUp)
                isUp = false;
            else
                isUp = true;
        }
        if(isBtnOn)
        {
            if(targetPosition != topPosition)
                targetPosition = topPosition;
        }
        else
        {
            if (isUp)
            {
                targetPosition = transform.position + movePosition;
            }
            else
            {
                targetPosition = transform.position - movePosition;
            }
        }


        curWaitTime = 0f;
        isMoving = true;
        curMoveCount++;
    }
    private void MovingToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if ((transform.position - targetPosition).sqrMagnitude < 0.001f) // 오차 범위 내에 도달
        {
            transform.position = targetPosition; // 최종 위치 보정
            curWaitTime += Time.deltaTime;

            if (curWaitTime > waitingTime)
            {
                if(isBtnOn && transform.position == topPosition)
                {
                    isBtnOn = false;
                    curMoveCount = moveCount;
                }
                isMoving = false; // 이동 완료
            }
        }
    }

    public void Exe()
    {
        targetPosition = buttonTransforms[btnNum].position;
        curWaitTime += waitingTime;

        isBtnOn = true; 
        isMoving = true;
        isUp = true;
        curWaitTime = 0f;
        Debug.Log("엘리베이터 상호작용");

    }
}
