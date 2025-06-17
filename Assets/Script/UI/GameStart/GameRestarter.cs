using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{
    public void RestartGame()
    {
        SingletonDestroyer.DestroyAllSingletons();
        SceneManager.LoadScene("GameStartScene");
    }
}