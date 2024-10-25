using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HUDController : MonoBehaviour
{
    [SerializeField] GameObject SlotText;

    void Start()
    {
        SlotText.SetActive(false);
    }
}
