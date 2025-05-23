using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTemp : MonoBehaviour
{
    [SerializeField] private string SceneName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SceneManager.LoadScene(SceneName);
    }
}
