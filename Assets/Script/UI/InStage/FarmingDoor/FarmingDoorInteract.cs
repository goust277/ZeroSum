using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingDoorInteract  : MonoBehaviour
{
    [SerializeField] private GameObject[] invisibleObjs;

    public void SetInvisible(bool isInvisible)
    {
        foreach (GameObject obj in invisibleObjs)
        {
            if (obj != null)
            {
                obj.SetActive(isInvisible);
            }
        }
    }
}
