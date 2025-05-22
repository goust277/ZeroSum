using Cinemachine;
using Com.LuisPedroFonseca.ProCamera2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialBox : MonoBehaviour
{

    [Header("CutsceneManager Resources")]
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private PlayableDirector director;

    private int dmg;
    [SerializeField] private Box box;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject Num4Obj;
    //[SerializeField] private CutsceneManager cutsceneManager;
    private Vector3 initialCameraPosition;
    private bool hasPlayed = false;


    void Update()
    {

        if (box == null)
        {
            box = GetComponent<Box>(); // Ȱ��ȭ�Ǿ��� �� �� ���� ������
            return; // ���� �ʱ�ȭ ���̴ϱ� ������
        }

        dmg = box.hitCount;
        if (dmg == 2 && !hasPlayed)
        {
            hasPlayed = true;
            BoxOpen();
        }
    }

    private void BoxOpen()
    {
        // ���� ī�޶� ��ġ�� ȸ������ �����ͼ� ����� ī�޶� ����
        Vector3 currentCameraPosition = Camera.main.transform.position;
        Quaternion currentCameraRotation = Camera.main.transform.rotation;

        // ����� ī�޶��� ��ġ�� ȸ������ ���� ī�޶� ������ ����
        virtualCamera.transform.position = currentCameraPosition;
        virtualCamera.transform.rotation = currentCameraRotation;
        //cutsceneManager.PlayCutscene(director);
        if (director != null)
        {
            cutsceneManager.PlayCutscene(director);
        }
        Num4Obj.SetActive(true);
    }

}
