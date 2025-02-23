using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AdjustSpriteSize : MonoBehaviour
{
    public float fixedMaxSize = 120f; // �ִ� ũ�� ���� (120�̵�, 90�̵� ���� ����)
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetSprite();
    }

    public void SetSprite()
    {
        Image image = GetComponent<Image>(); // �Ǵ� �ʿ��� Image ������Ʈ�� ��������
        Sprite sprite = image.sprite; // Image���� ��������Ʈ�� ������

        if (sprite == null) return;

        // ��������Ʈ�� ���� ���� �����ϸ鼭 �ִ� ũ�⸦ ����
        float widthRatio = sprite.rect.width / sprite.rect.height;

        // ū ���� fixedMaxSize�� ���߰�, ������ ���� ���� ���� ����
        if (widthRatio > 1)
        {
            rectTransform.sizeDelta = new Vector2(fixedMaxSize, fixedMaxSize / widthRatio);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(fixedMaxSize * widthRatio, fixedMaxSize);
        }
    }
}
