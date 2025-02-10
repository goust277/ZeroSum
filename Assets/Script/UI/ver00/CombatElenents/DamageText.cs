using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private Vector3 startPos; // 초기 위치 저장
    [SerializeField] private TextMeshProUGUI text; // 텍스트 컴포넌트
    [SerializeField] private float fadeDuration = 4.0f; // 텍스트가 완전히 사라지기까지의 시간
    private float alpha = 1.0f; // 알파값 초기화
    [SerializeField] private float moveSpeed = 0.5f; // Y축 이동 속도
    private float time = 0.0f;
    
    private void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Y축 이동
        float yOffset = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(0, yOffset, 0);

        // 알파값 감소
        alpha -= Time.deltaTime / fadeDuration;
        alpha = Mathf.Clamp01(alpha); // 알파값이 0~1 사이로 유지되도록 보장

        // 텍스트 색상 변경 (알파값 적용)
        if (text != null)
        {
            Color currentColor = text.color;
            text.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        }

        // 이동 거리와 알파값 체크 후 파괴
        if (time >= fadeDuration)
        {
            Destroy(gameObject);
        }

        time += Time.deltaTime;

    }
}
