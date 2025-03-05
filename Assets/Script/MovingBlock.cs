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
    [Header("������")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float waitingTime;
    [SerializeField] private float moveCount;

    [Header("ù �����̴� ��")]
    [SerializeField] private float firstMoveCount;

    private float curMoveCount;
    private float curWaitTime;
    private bool isMoveUp;

    private Vector3 targetPostion;

    private bool isMoving = false;

    private void Start()
    {
        targetPostion = transform.position;
        curWaitTime = 0.0f;
        curMoveCount = 0f;
        isMoveUp = true;
        if (firstMoveCount == 0f)
            firstMoveCount = moveCount;
    }

    // Update is called once per frame
    private void Update()
    {

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
            if (isMoveUp)
            {
                isMoveUp = false;
                curMoveCount = 0f;
            }
            else
            {
                isMoveUp = true;
                curMoveCount = 0f;
            }
        }


        if(isMoveUp)
        {
            targetPostion = transform.position + movePosition;
        }
        else 
        {
            targetPostion = transform.position - movePosition;
        }

        curWaitTime = 0f;
        isMoving = true;
        curMoveCount++;
    }

    private void MovingToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPostion, moveSpeed * Time.deltaTime);

        if ((transform.position - targetPostion).sqrMagnitude < 0.001f) // ���� ���� ���� ����
        {
            transform.position = targetPostion; // ���� ��ġ ����
            curWaitTime += Time.deltaTime;

            if (curWaitTime > waitingTime)
                isMoving = false; // �̵� �Ϸ�
        }
    }
}
