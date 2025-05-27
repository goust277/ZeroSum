using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtkCol : MonoBehaviour
{
    [SerializeField] private GameObject[] col;
    // Start is called before the first frame update
    public void OnDownAtk()
    {
        col[1].gameObject.SetActive(true);
    }

    [System.Obsolete]
    public void OffDownAtk()
    {
        if (col[1].gameObject.active)
            col[1].gameObject.SetActive(false);
    }

    public void OnStandAtk() 
    {
        col[0].gameObject.SetActive(true);
    }
    public void OffStandAtk()
    {
        col[0].gameObject.SetActive(false);
    }
}
