using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private float time;
    [Header("움직임")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float waitingTime;
    [SerializeField] private float moveCount;

    [Header("첫 움직이는 수")]
    [SerializeField] private float firstMoveCount;

    private float curMoveCount;
    private float curWaitTime;
    private bool isMoveUp;

    private Vector3 targetPostion;

    private bool isMoving = false;

    public bool isBottom;

    private void Start()
    {
        targetPostion = transform.position;
        curWaitTime = 0.0f;
        curMoveCount = 0f;
        isMoveUp = true;
        if (firstMoveCount == 0f)
            firstMoveCount = moveCount;
        isBottom = false;
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

        if ((transform.position - targetPostion).sqrMagnitude < 0.001f) // 오차 범위 내에 도달
        {
            transform.position = targetPostion; // 최종 위치 보정
            curWaitTime += Time.deltaTime;

            if (curWaitTime > waitingTime)
                isMoving = false; // 이동 완료
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isBottom)
        {
            IDamageAble damageAble = collision.GetComponent<IDamageAble>();
            if (damageAble != null)
            {
                damageAble.Damage(10);
            }
        }
    }
}
