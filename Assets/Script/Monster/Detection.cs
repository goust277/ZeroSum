using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Detection : MonoBehaviour
{
    private IDetectable detectable; // �������̽��� ���� ��ü�� ���

    void Start()
    {
        // �θ� ��ü���� IDetectable �������̽��� ������ ��ũ��Ʈ�� ã��
        detectable = GetComponentInParent<IDetectable>();

        if (detectable == null)
        {
            Debug.LogError("IDetectable �������̽��� ������ ��ũ��Ʈ�� �θ� ��ü���� ã�� �� �����ϴ�!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detectable?.SetPlayerInRange(true);
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        detectable?.SetPlayerInRange(false);
    //    }
    //}
}
