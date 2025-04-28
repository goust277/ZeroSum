using Cinemachine;
using Com.LuisPedroFonseca.ProCamera2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class TutorialBox : MonoBehaviour
{
    private int dmg;
    [SerializeField] private Box box;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject Num4Obj;
    //[SerializeField] private CutsceneManager cutsceneManager;
    private Vector3 initialCameraPosition;
    private bool hasPlayed = false;


    void Update()
    {

        if (box == null)
        {
            box = GetComponent<Box>(); // 활성화되었을 때 한 번만 가져와
            return; // 아직 초기화 중이니까 나가기
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
        // 현재 카메라 위치와 회전값을 가져와서 버츄얼 카메라에 적용
        Vector3 currentCameraPosition = Camera.main.transform.position;
        Quaternion currentCameraRotation = Camera.main.transform.rotation;

        // 버츄얼 카메라의 위치와 회전값을 현재 카메라 값으로 설정
        virtualCamera.transform.position = currentCameraPosition;
        virtualCamera.transform.rotation = currentCameraRotation;
        //cutsceneManager.PlayCutscene(director);
        director.Play();
        Num4Obj.SetActive(true);
    }

}
