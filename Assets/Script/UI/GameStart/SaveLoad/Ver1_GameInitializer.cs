using UnityEngine;
using UnityEngine.SceneManagement;

public class Ver1_GameInitializer : MonoBehaviour
{
    private LoadSaveFile saveFileLoader;

    private void Awake()
    {
        saveFileLoader = new LoadSaveFile();

        PlayerState playerState = saveFileLoader.LoadPlayerState();
        if (playerState != null)
        {
            Debug.Log("GameInitializer - Awake // saveFileLoader.LoadPlayerState() is not null ");
            InitializeGame(playerState);
        }
        else
        {
            Debug.Log("GameInitializer - Awake // saveFileLoader.LoadPlayerState() is null ");
            SceneManager.LoadScene("SampleSceneUI"); // ù �� �ε�
        }
    }

    private void InitializeGame(PlayerState playerState)
    {
        // 1. �� �ε�
        int sceneNum = playerState.Version.scene;
        SceneManager.LoadScene(sceneNum);

        // 2. �÷��̾� ��ġ
        Vector3 startPosition = new Vector3(playerState.position.x, playerState.position.y, 0);
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = startPosition;
        }

        // 4. �ɼ� ����
        ApplySettings(playerState.settings);

        // 5. �̺�Ʈ �÷��� ����
        EventRoot eventRoot = saveFileLoader.LoadEventRoot();
        if (eventRoot != null)
        {
            Event chapterEvent = eventRoot.Events.Find(e => e.chapterNum == playerState.Version.chapter);
            GameStateManager.Instance.currentEventFlags = chapterEvent?.EventFlags;

            Debug.LogWarning("");
        }

        GameStateManager.Instance.SetCurrenSceneID(playerState.Version.currentSceneID);
        //GameStateManager.Instance.SetStagePoint(playerState.Version.currentStagePoint);
        Debug.Log("���� �̾��ϱ� �غ� �Ϸ�");
    }

    private void ApplySettings(Settings settings)
    {
        PlayerPrefs.SetFloat("BackgroundVolume", settings.BackgroundVolume);
        PlayerPrefs.SetFloat("EffectsVolume", settings.EffectVolume);
    }
}
