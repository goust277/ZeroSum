using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReTry : MonoBehaviour
{
    public void OnClickRestart()
    {
        Time.timeScale = 1; // 게임 재개
        //Ver01_DungeonStatManager.Instance.ResetDungeonState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
