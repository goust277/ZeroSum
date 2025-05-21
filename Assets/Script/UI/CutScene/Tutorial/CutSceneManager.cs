using UnityEngine;
using UnityEngine.Playables;
using Com.LuisPedroFonseca.ProCamera2D;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] public Transform playerTransform;
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

            // 현재 카메라 사이즈와 ProCamera2D 줌을 맞춤
            float targetZoom = Camera.main.orthographicSize;

            proCamera2D.enabled = true;

            if (targetZoom > 3.6f)
            {
                //Debug.Log("orthographicSize : " + targetZoom);
                float deltaZoom = (targetZoom / 2.01f); // 상대 변화량
               // Debug.Log("deltaZoom : " + deltaZoom);
                proCamera2D.Zoom(deltaZoom); // 적용
                //Debug.Log("orthographicSize : " + Camera.main.orthographicSize);
            }
        }

        playerController.SetActive(true);
        director.stopped -= OnCutsceneEnd;
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

}
