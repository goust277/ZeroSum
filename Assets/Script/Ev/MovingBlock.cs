using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingBlock : MonoBehaviour
{
    private float time;
    [Header("움직임")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float waitingTime;
    [SerializeField] private float moveCount;


    private float curMoveCount;
    private float curWaitTime;
    private bool isMoveLastTarget;
    private bool isUp;
    private bool isWaiting;
    public bool speedUp;
    public bool inPlayer;

    private Vector3 targetPostion;
    private Vector3 firstPosition;
    private Vector3 lastPostion;

    private bool isMoving = false;

    [HideInInspector] public bool isBottom;

    private void Start()
    {
        targetPostion = transform.position;
        curWaitTime = 0.0f;
        curMoveCount = 0f;
        isMoveLastTarget = true;
        speedUp = false;
        isBottom = false;
        isWaiting = false;
        firstPosition = transform.position;
        lastPostion = transform.position + (movePosition * moveCount);
    }

    // Update is called once per frame
    private void Update()
    {
        if (curMoveCount == moveCount)
        {
            if (transform.position.y - targetPostion.y > 0f)
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

        if ((transform.position.y - targetPostion.y) < 0f)
        {
            if (!isUp)
                isUp = true;
        }
        else if ((transform.position.y - targetPostion.y) > 0f)
        {
            if (isUp)
                isUp = false;
        }
        if (inPlayer)
        {
            if(targetPostion == firstPosition || targetPostion == lastPostion)
            {
                speedUp = false;
            }

            if (isUp) //위
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (!speedUp)
                    {
                        speedUp = true;
                        Debug.Log("스피드 위");
                    }
                }
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    if (speedUp)
                        speedUp = false;
                }

            }
            else if (!isUp)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (!speedUp)
                        speedUp = true;
                }
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    if (speedUp)
                        speedUp = false;
                }
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
            if (isMoveLastTarget)
            {
                isMoveLastTarget = false;
                curMoveCount = 0f;
            }
            else
            {
                isMoveLastTarget = true;
                curMoveCount = 0f;
            }
        }


        if(isMoveLastTarget)
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

        if ((transform.position - targetPostion).sqrMagnitude < 0.001f) // 오차 범위 내에 도달
        {
            if(!speedUp)
            {
                transform.position = targetPostion; // 최종 위치 보정
                curWaitTime += Time.deltaTime;

                isWaiting = true;
                if (speedUp)
                    curWaitTime += waitingTime;
                if (curWaitTime > waitingTime)
                {
                    isMoving = false; // 이동 완료
                    isWaiting = false;
                }
                    
            }
            else if(speedUp)
            {
                transform.position = targetPostion; // 최종 위치 보정
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            inPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
        }
    }
}
