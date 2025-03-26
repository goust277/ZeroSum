using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MissionDoorManager : MonoBehaviour
{
    [SerializeField] private List<MissionDoor> missionDoors = new List<MissionDoor>();
    [HideInInspector] public int clearMission;
    private int maxMission;

    [SerializeField] private GameObject missionWall;
    private void Start()
    {
        maxMission = missionDoors.Count;
        clearMission = 0;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if(clearMission == maxMission)
        {
            MissionClear(); //미션 클리어
            clearMission++;
        }
    }

    [System.Obsolete]
    private void MissionClear()
    {
        if(missionWall.active) 
        {
            missionWall.SetActive(false);
        }
    }
}
