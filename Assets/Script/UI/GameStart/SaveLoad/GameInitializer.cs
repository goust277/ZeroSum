using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    private LoadSaveFile saveFileLoader;

    private void Awake()
    {
        saveFileLoader = new LoadSaveFile();

        // 세이브 데이터 로드
        PlayerState playerState = saveFileLoader.LoadPlayerState();
        if (playerState != null)
        {
            //이어하기
            InitializeGame(playerState);
        }
        else
        {
            //새로하기
            SceneManager.LoadScene("SampleSceneUI"); // 첫 씬 로드
        }
    }

    private void InitializeGame(PlayerState playerState)
    {
        // 1. 씬 로드
        int sceneNum = playerState.Version.scene;
        SceneManager.LoadScene(sceneNum);

        // 2. 플레이어 배치
        Vector3 startPosition = new Vector3(playerState.position.x, playerState.position.y, 0);
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = startPosition;
        }

        // 3. 무기 추가
        foreach (int weaponID in playerState.weapons)
        {
            WeaponManager.Instance.AddAcquiredItemIds(weaponID);
        }

        // 4. 옵션 적용
        ApplySettings(playerState.settings);

        // 5. 이벤트 플래그 적용
        EventRoot eventRoot = saveFileLoader.LoadEventRoot();
        if (eventRoot != null)
        {
            Event chapterEvent = eventRoot.Events.Find(e => e.chapterNum == playerState.Version.chapter);
            GameStateManager.Instance.currentEventFlags = chapterEvent?.EventFlags;

            Debug.LogWarning("");
        }

        GameStateManager.Instance.SetCurrenSceneID(playerState.Version.currentStorySceneID);

        Debug.Log("게임 이어하기 준비 완료");
    }

    private void ApplySettings(Settings settings)
    {
        PlayerPrefs.SetInt("language", settings.language);
        PlayerPrefs.SetFloat("brightness", settings.brightness);
        PlayerPrefs.SetFloat("BackgroundVolume", settings.BackgroundVolume);
        PlayerPrefs.SetFloat("EffectsVolume", settings.EffectVolume);
        Debug.Log("옵션 값 적용 완료");
    }
}
