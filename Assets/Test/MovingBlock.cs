using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private float time;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float waitingTime;
    private float curWaitTime;

    private Vector3 targetPostion;
    private Vector3 startPosition;

    private bool isMoving = false;
    private bool isMoveReady = true;

    private void Start()
    {
        targetPostion = transform.position;
        startPosition = transform.position;
        curWaitTime = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isMoving)
            MoveToTarget();
        else
            MovingToTarget();
    }

    private void MoveToTarget()
    {
        if (transform.position != startPosition)
        {
            targetPostion = startPosition;
        }
        else
            targetPostion = transform.position + movePosition;

        curWaitTime = 0f;
        isMoving = true;
    }

    private void MovingToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPostion, moveSpeed * Time.deltaTime);

        if ((transform.position - targetPostion).sqrMagnitude < 0.001f) // 오차 범위 내에 도달
        {
            transform.position = targetPostion; // 최종 위치 보정
            curWaitTime += Time.deltaTime;

            if (curWaitTime > waitingTime)
                isMoving = false; // 이동 완료
        }
    }
}
