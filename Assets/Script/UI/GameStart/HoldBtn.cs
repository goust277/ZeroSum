using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HoldBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    protected Vector3 originalScale = new Vector3(1.0f, 1.0f, 1f);
    protected bool isSelected = false;
    [SerializeField] private Sprite selectedSlotSprite; // ���õ� ��ư ����
    [SerializeField] private Sprite defaultSSlotprite;  // �⺻ ��ư ����
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    private Image image;
    private Color newColor;

    void Start()
    {
        image = GetComponent<Image>();
        newColor = image.color;
        newColor.a = 0.95f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            image.sprite = selectedSlotSprite;
            image.color = newColor;
            transform.localScale = hoverScale;
            isSelected = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected)
        {
            image.sprite = defaultSSlotprite;
            image.color = newColor;
            // ���콺�� ������Ʈ�� ���� �� ������ ������� ����
            transform.localScale = originalScale;
            isSelected = false;
        }
    }
}
