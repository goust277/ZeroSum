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
        Num4Obj.SetActive(true);
    }
}
