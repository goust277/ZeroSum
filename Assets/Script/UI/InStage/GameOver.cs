using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Transform offsetTarget;
    [SerializeField] private float targetSize = 2.0f;
    [SerializeField] private float duration = 2.0f;
    [SerializeField] Image redImg;
    [SerializeField] Image blackImg;
    [SerializeField] GameObject gameOverUI;
    private Transform player;
    private GameObject camObj;
    private Camera cam;
    private ProCamera2D proCamera2D;
    

    private void Start()
    {
        camObj = GameObject.FindWithTag("MainCamera");
        if (camObj == null)
        {
            Debug.LogError("GameOver - 카메라 못찾음 ");
            return;
        }

        cam = camObj.GetComponent<Camera>();
        proCamera2D = camObj.GetComponent<ProCamera2D>();

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다.");
            return;
        }
        player = playerObj.transform;

        StartCoroutine(HandleGameOverSequence());
        proCamera2D.Zoom(-5.0f, 2.0f, EaseType.EaseInOut);
    }


    IEnumerator HandleGameOverSequence()
    {

        Color color = redImg.color;
        offsetTarget.transform.position = player.position + new Vector3(0f, 0.5f, 0f);

        proCamera2D.RemoveAllCameraTargets();
        proCamera2D.AddCameraTarget(offsetTarget);

        float targetSize = 2.0f;
        float startSize = cam.orthographicSize;
        float time = 0f;

        while (time < duration)
        {

            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 0.9f, time / duration);
            redImg.color = new Color(color.r, color.g, color.b, alpha);
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, time / duration);
            yield return null;
        }


        //cam.orthographicSize = targetSize; // 최종 사이즈 보정
        gameOverUI.SetActive(true);

        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 0.9f, time / duration);
            blackImg.color = new Color(color.r, color.g, color.b, alpha);
            //cam.transform.position = player.transform.position;
            yield return null;
        }
    }
}