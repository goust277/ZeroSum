using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MissionDoorManager : MonoBehaviour
{
    [SerializeField] private List<MissionDoor> missionDoors = new List<MissionDoor>();
    public int clearMission;
    private int maxMission;
    public int curMission;

    [SerializeField] private GameObject missionWall;

    [SerializeField] private TextMeshProUGUI txt;
    private void Start()
    {
        maxMission = missionDoors.Count;
        clearMission = 0;
        curMission = 0;
        txt.text = clearMission.ToString() + " / " + maxMission.ToString();
    
        //튜토리얼때문에 초기화 코드
        txt.color = Color.white;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (curMission != clearMission)
        {
            txt.text = clearMission.ToString() + " / " + maxMission.ToString();
            curMission = clearMission;
        }
        if(clearMission == maxMission)
        {
            MissionClear(); //미션 클리어
            txt.text = "Clear";
            txt.color = Color.green;
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
