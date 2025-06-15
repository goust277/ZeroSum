using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ElvButtonGuide : MonoBehaviour
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
        //playerInput = GameObject.Find("InputManager");

        //if(playerInput != null)
        //{
        //    interactAction = playerInput.GetComponent<PlayerInput>().actions["F"];
        //}
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //if(interactAction == null)
            //{
            //    interactAction = playerInput.GetComponent<PlayerInput>().actions["F"];
            //}

            //guideUIObject.text = "엘리베이터 호출\n ";
            //guideUIObject.text += "[" + interactAction.bindings[0].ToDisplayString() + "]";

            //guideUIObject.enabled = true;

            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(false);
            //guideUIObject.enabled = false;
        }
    }
}
