using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{
    public void RestartGame()
    {
        DestroyAllSingletons();  // ���� SingletonDestroyer �� �ᵵ ��
        SceneManager.LoadScene("GameStartScene");
    }

    private void DestroyAllSingletons()
    {
        var allObjects = FindObjectsOfType<MonoBehaviour>(true); // ��Ȱ�� ����

        foreach (var obj in allObjects)
        {
            if (obj is ISingleton singleton)
            {
                singleton.OnSingletonDestroy();
            }
        }
    }
}
