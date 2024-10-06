using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Qslot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Vector3 originalScale = new Vector3(1.0f, 1.0f, 1f);
    protected bool isSelected = false;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    //private ClockWork Slot;
    [SerializeField] protected TextMeshProUGUI nameTXT;
    [SerializeField] protected TextMeshProUGUI desTXT;
    [SerializeField] protected GameObject InfBG;

    public virtual void OnPointerEnter(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected)
        {
            // 마우스가 오브젝트를 떠날 때 스케일 원래대로 복원
            transform.localScale = originalScale;
            isSelected = false;
            InfBG.SetActive(false);
        }
    }
}
