using System.Reflection;
using UnityEngine;

public class MissionDoor : BaseInteractable
{
    [SerializeField] private MissionDoorManager doorManager;
    [SerializeField] private GameObject monsterDoor;

    [SerializeField] private GameObject mission;

    private bool isClear;
    public override void Exe()
    {
        mission.GetComponent<PipeManager>().OnMission();
    }

    private void Update()
    {
        if (mission.GetComponent<PipeManager>().isClear)
        {
            if(!isClear) 
            {
                isClear = true;
                doorManager.clearMission++;
                monsterDoor.SetActive(true);
            }
        }
    }

}
