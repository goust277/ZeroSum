using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadSetting : MonoBehaviour
{
    public bool isMissionStart = false;
    public bool isMissionClear = false;
    
    void Start()
    {
        GameStateManager.Instance.StartMoveUIDown();   
    }
}
