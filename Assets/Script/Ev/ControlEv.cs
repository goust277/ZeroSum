using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEv : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float waitingTime;
    [SerializeField] private float moveCount;

    [Header("Button")]
    [SerializeField] private GameObject[] evBtns;

    private float curMoveCount;
    private float curWaitTime;
    private bool isUp;
    private bool isMoving = false;
    private bool isBtnOn;
    [HideInInspector] public bool isBottom;
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
        if(isBtnOn)
        {
            
            if (!isMoving)
                MoveToTarget();
            else
                MovingBtns();
        }
        else 
        {
            if (!isMoving)
                MoveToTarget();
            else
                MovingToTarget();
        }

    }
    private void MoveToTarget()
    {
        if (curMoveCount == moveCount)
        {
            curMoveCount = 0f;
            movePosition.y *= -1f;
        }

        targetPosition = transform.position + movePosition;

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
                isMoving = false; // 이동 완료
            }
        }
    }

    private void MovingBtns()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if ((transform.position - targetPosition).sqrMagnitude < 0.001f) // 오차 범위 내에 도달
        {
            transform.position = targetPosition; // 최종 위치 보정
            curWaitTime += Time.deltaTime;

            if (curWaitTime > waitingTime)
            {
                isMoving = false;
                if(transform.position == topPosition)
                {
                    
                }
            }
        }
    }
    private void MoveToTop()
    {
        targetPosition = topPosition;
    }
    private void ResetMove()
    {
       
    }
    public void MoveBtn(Vector3 pos, int floor)
    {
        targetPosition = pos;

        curWaitTime += waitingTime;

        isBtnOn = true;
        isMoving = true;
    }
}
