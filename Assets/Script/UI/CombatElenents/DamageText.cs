using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private Vector3 startPos; // �ʱ� ��ġ ����
    [SerializeField] private TextMeshProUGUI text; // �ؽ�Ʈ ������Ʈ
    [SerializeField] private float fadeDuration = 1.0f; // �ؽ�Ʈ�� ������ ������������ �ð�
    private float alpha = 1.0f; // ���İ� �ʱ�ȭ
    [SerializeField] private float moveSpeed = 0.5f; // Y�� �̵� �ӵ�
    private void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Y�� �̵�
        float yOffset = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(0, moveSpeed, 0);

        // ���İ� ����
        alpha -= Time.deltaTime / fadeDuration;
        alpha = Mathf.Clamp01(alpha); // ���İ��� 0~1 ���̷� �����ǵ��� ����

        // �ؽ�Ʈ ���� ���� (���İ� ����)
        if (text != null)
        {
            Color currentColor = text.color;
            text.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        }

        // �̵� �Ÿ��� ���İ� üũ �� �ı�
        if (alpha <= 0.0f || Vector3.Distance(startPos, transform.position) >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
