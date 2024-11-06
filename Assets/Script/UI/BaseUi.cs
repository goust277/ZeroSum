using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUi : MonoBehaviour
{
    [SerializeField] protected Button[] btns;

    protected virtual void Awake()
    {
        for (int i = 0; i < btns.Length; i++) 
        {
            string name = btns[i].name;
            btns[i].onClick.AddListener(() => { ButtonOnClick(name); });
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void ButtonOnClick(string btnName)
    {
        ButtonFuncion(btnName);
    }

    protected virtual void ButtonFuncion(string btnName)
    {

    }
}
