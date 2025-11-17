using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class TutorialSummoner : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInput;

    //private int health = 3;

    [SerializeField] private GameObject box;
    [SerializeField] private TextMeshProUGUI boxText;

    private int summoner;

    void Update()
    {
        summoner = gameObject.GetComponent<Scout>().health;
        if (summoner <= 0)
        {
            BoxOn();
        }
    }

    private void BoxOn()
    {
        //InputAction action = playerInput.actions["SwordAttack"];
        //string key = action.bindings[0].ToDisplayString();

        boxText.text = "ÁÂÅ¬¸¯À» ´­·¯ º¸±Þ »óÀÚ ÆÄ±«";

        box.SetActive(true);

        //¾ÈÁ×´Â°Å ´ëºñÇØ¼­ °Á ºÎ¼û
        Destroy(gameObject, 3f);
    }
}
