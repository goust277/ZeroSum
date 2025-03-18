using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEv : MonoBehaviour
{
    [Header("¿òÁ÷ÀÓ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movePosition;

    private bool isMoveUp;
    private bool isPlayerIn;

    private Vector3 targetPostion;
    void Start()
    {
        isMoveUp = false;
        isPlayerIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        List<int> list = new List<int>();
        int a = 1;
        list[a + 1] = a;
    }
}
