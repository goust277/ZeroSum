using UnityEngine;
using UnityEngine.Playables;
using Com.LuisPedroFonseca.ProCamera2D;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject playerController; // ��ũ��Ʈ�� MonoBehaviour�� �����ؼ� �ޱ�

    private GameObject camObj;
    private ProCamera2D proCamera2D;

    private void Start()
    {
        camObj = GameObject.FindWithTag("MainCamera");
        if (camObj == null)
        {
            Debug.LogError("GameOver - ī�޶� ��ã�� ");
            return;
        }
        proCamera2D = camObj.GetComponent<ProCamera2D>();
    }

    public void PlayCutscene(PlayableDirector cutsceneDirector)
    {
        if (cutsceneDirector == null)
        {
            Debug.Log("CutsceneManager: ����� Cutscene�� �����ϴ�.");
            return;
        }

        director = cutsceneDirector;

        if (proCamera2D != null)
            proCamera2D.enabled = false;

        if (playerController != null)
            playerController.SetActive(false);

        //director.Play();
        director.stopped += OnCutsceneEnd;
    }

    public void OnCutsceneEnd(PlayableDirector obj)
    {
        if (proCamera2D != null)
        {
            proCamera2D.enabled = true;
            proCamera2D.RemoveAllCameraTargets();
            proCamera2D.AddCameraTarget(playerTransform);
        }

        playerController.SetActive(true);

        director.stopped -= OnCutsceneEnd;
    }
}
