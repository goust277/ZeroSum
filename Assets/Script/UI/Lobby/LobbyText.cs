using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyText : MonoBehaviour
{
    private Material tmpMaterial;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.outlineWidth = 0.5f; // �ܰ��� �β�
        text.outlineColor = Color.cyan; // �׿� ����
    }

}
