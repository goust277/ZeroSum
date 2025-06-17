using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SingletonDestroyer
{
    public static void DestroyAllSingletons()
    {
        var allRootObjects = Object.FindObjectsOfType<GameObject>(true);

        foreach (var obj in allRootObjects)
        {
            if (obj.scene.name == null || obj.scene.name == "")
            {
                Object.Destroy(obj);
            }
        }
    }
}
