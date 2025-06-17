using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public void RetryFromButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
