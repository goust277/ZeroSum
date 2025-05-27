using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    private int dmg;
    [SerializeField] private Box box;
    [SerializeField] private GameObject Num4Obj;
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
        Num4Obj.SetActive(true);
    }
}
