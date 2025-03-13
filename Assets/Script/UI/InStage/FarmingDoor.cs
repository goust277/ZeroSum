using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class FarmingDoor : MonoBehaviour
{
    public Transform[] items; // 룰렛 안에 있는 4개의 아이템
    public float scrollSpeed = 0.1f; // 스크롤 속도
    public float spinDuration = 10f; // 슬롯이 굴러갈 시간 (1초)
    private bool isSpinning = false;
    public int rewardIndex = 0; // 당첨 아이템의 인덱스


    private bool isTriggerEnter = false;
    [SerializeField] private GameObject dropItem;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTriggerEnter)
            {
                StartSpin();
                isTriggerEnter = true;
                spriteRenderer.color = Color.gray; // 색상을 회색으로 변경
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
        }
    }


    // 슬롯 회전 시작
    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinSlots());
        }
    }

    // 슬롯 회전
    private IEnumerator SpinSlots()
    {
        isSpinning = true;

        // 회전 초기화
        float elapsedTime = 0f;
        int currentItemIndex = 0;

        while (elapsedTime < spinDuration)
        {
            // 룰렛 회전: 각 아이템을 회전시켜서 보이게 함
            transform.Rotate(Vector3.forward, scrollSpeed * Time.deltaTime);
            currentItemIndex = (currentItemIndex + 1) % items.Length;

            // 각 아이템 순차적으로 보이게 만들기
            for (int i = 0; i < items.Length; i++)
            {
                items[i].gameObject.SetActive(i == currentItemIndex);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회전 멈추기
        StopRoulette();
    }


    // 룰렛 멈추고 당첨 아이템 출력
    private void StopRoulette()
    {
        Debug.Log("당첨 아이템: " + items[rewardIndex].name);
        isSpinning = false;
    }


}

