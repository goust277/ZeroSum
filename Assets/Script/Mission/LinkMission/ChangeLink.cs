using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLink : MonoBehaviour
{
    [SerializeField] private Sprite aSprite;
    [SerializeField] private Sprite qSprite;
    [SerializeField] private Image image;

    private bool isAnswer = false;
    public void ChangeColor()
    {
        if (!isAnswer)
        {
            isAnswer = true;
            image.sprite = aSprite;

        }
        else if (isAnswer)
        {
            isAnswer = false;
            image.sprite = qSprite;
        }
    }

    public void SetSprite()
    {
        image.sprite = aSprite;
    }

    public void ResetSprite()
    {
        image.sprite = qSprite;
    }
}
