using UnityEngine;

public class MissionDoor : BaseInteractable
{
    [SerializeField] private MissionDoorManager doorManager;
    [SerializeField] private GameObject monsterDoor;

    [SerializeField] private Mission mission;

    private bool isClear;
    public override void Exe()
    {
        mission.OnMission();

    }

    private void Update()
    {
        if (mission.isClear)
        {
            doorManager.clearMission++;
            if(monsterDoor != null ) 
            {
                monsterDoor.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}
