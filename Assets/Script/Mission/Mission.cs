using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    public GameObject player;
    public GameObject input;
    public bool isClear;
    public bool isFailed;
    public abstract void OnMission();
}
