using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
    [SerializeField] public GameObject toActiveObj;
    private void OnDisable()
    {
        Debug.Log($"{gameObject.name} 비활성화됨 → 활성화 시도");

        if( toActiveObj != null)
        {
            toActiveObj.SetActive(true);
        }    
    }
}
