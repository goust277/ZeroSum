using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AdjustSpriteSize : MonoBehaviour
{
    public float fixedMaxSize = 120f; // 최대 크기 지정 (120이든, 90이든 설정 가능)
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
        Image image = GetComponent<Image>(); // 또는 필요한 Image 컴포넌트를 가져오고
        Sprite sprite = image.sprite; // Image에서 스프라이트를 가져옴

        if (sprite == null) return;

        // 스프라이트의 원래 비율 유지하면서 최대 크기를 설정
        float widthRatio = sprite.rect.width / sprite.rect.height;

        // 큰 쪽을 fixedMaxSize로 맞추고, 비율에 맞춰 작은 쪽을 조정
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
