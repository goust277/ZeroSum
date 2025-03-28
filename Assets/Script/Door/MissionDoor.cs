using UnityEngine;

public class MissionDoor : BaseInteractable
{
    [SerializeField] private MissionDoorManager doorManager;
    [SerializeField] private GameObject monsterDoor;
    public override void Exe()
    {
        doorManager.clearMission++;
        monsterDoor.SetActive(true);
    }

}
