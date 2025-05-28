using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEv : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movePosition;

    [Header("InputManager")]
    [SerializeField] private GameObject inputManager;
    private bool isTop;
    private bool isMoving;

    private Vector3 targetPostion;
    void Start()
    {
        targetPostion = transform.position;
        isMoving = false;
        isTop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(inputManager == null)
        {
            inputManager = GameObject.Find("InputManager");
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
        if(!isTop)
        {
            targetPostion = transform.position + movePosition;
        }
        else
        {
            targetPostion = transform.position - movePosition;
        }
    }
    private void MovingToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPostion, moveSpeed * Time.deltaTime);

        if ((transform.position - targetPostion).sqrMagnitude < 0.001f) // ���� ���� ���� ����
        {
            transform.position = targetPostion; // ���� ��ġ ����
            isMoving = false;
            if (!isTop)
            {
                isTop = true;
                Debug.Log("�� ����");
            }
            else if(isTop)
            {
                isTop = false;
                Debug.Log("�Ʒ� ����");
            }
            inputManager.SetActive(true);
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player") && collision.gameObject.GetComponent<PlayerHit>())
            {
            //isMoving = true;
            //inputManager.SetActive(false);
            //Debug.Log("������ ��Ȱ��ȭ");
            if (gameObject.CompareTag("MovingBlock"))
            {
                isMoving = true;
                inputManager.SetActive(false);
            }
        }
        }
}
