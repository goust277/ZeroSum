using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{
    public void RestartGame()
    {
        DestroyAllSingletons();  // 굳이 SingletonDestroyer 안 써도 됨
        SceneManager.LoadScene("GameStartScene");
    }

    private void DestroyAllSingletons()
    {
        var allObjects = FindObjectsOfType<MonoBehaviour>(true); // 비활성 포함

        foreach (var obj in allObjects)
        {
            if (obj is ISingleton singleton)
            {
                singleton.OnSingletonDestroy();
            }
        }
    }
}
