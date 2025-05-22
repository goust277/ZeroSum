using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class CutSceneBase : MonoBehaviour
{
    [SerializeField] protected ProCamera2D proCamera2D;
    [SerializeField] protected Transform[] cutsceneTarget; // �ƾ� �� �� ����
     protected Transform playerTarget;
    [SerializeField] protected float zoomDuringCutscene = 4.5f;
    [SerializeField] protected float zoomSpeed = 1.5f;

    protected float originOrthographic = 6.7f;
    protected bool hasPlayed = false;

    protected void Start()
    {
        playerTarget = GameObject.Find("Player").transform;
    }

    protected void StartCutScene()
    {
        // �÷��̾� ����ٴϴ� �� ����
        proCamera2D.RemoveAllCameraTargets();
    }

    protected void EndCutScene()
    {
        float startZoom = Camera.main.orthographicSize;

        proCamera2D.AddCameraTarget(playerTarget, 0f, 2f);
        proCamera2D.Zoom(startZoom - originOrthographic, 2.0f);
    }
}
