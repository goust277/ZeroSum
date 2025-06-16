using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ButtonGuide : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI guideUIObject;
    [SerializeField] private string guideText;
    [SerializeField] private GameObject canvas;
    private InputAction interactAction; //0
    private GameObject playerInput;
    private void Start()
    {

        guideUIObject.text = guideText;
        canvas.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(false);
        }
    }
}
