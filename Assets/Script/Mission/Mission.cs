using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    public GameObject player;
    public GameObject input;
    public bool isClear;
    public abstract void OnMission();
}
