using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
    [SerializeField] public GameObject toActiveObj;
    private void OnDisable()
    {
        Debug.Log($"{gameObject.name} ��Ȱ��ȭ�� �� Ȱ��ȭ �õ�");

        if( toActiveObj != null)
        {
            toActiveObj.SetActive(true);
        }    
    }
}
