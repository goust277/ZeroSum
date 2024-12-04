using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Detection : MonoBehaviour
{
    private IDetectable detectable; // 인터페이스로 상위 객체와 통신

    void Start()
    {
        // 부모 객체에서 IDetectable 인터페이스를 구현한 스크립트를 찾음
        detectable = GetComponentInParent<IDetectable>();

        if (detectable == null)
        {
            Debug.LogError("IDetectable 인터페이스를 구현한 스크립트를 부모 객체에서 찾을 수 없습니다!");
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
