using UnityEngine;

public class Detection : MonoBehaviour
{
    private IDetectable detectable;

    //[SerializeField] private Transform eyePosition; // ������ �� ��ġ
    //[SerializeField] private LayerMask wall; // �� ���̾�

    //private Transform player;

    void Start()
    {
        detectable = GetComponentInParent<IDetectable>();

        if (detectable == null)
        {
            Debug.LogError("�θ� ��ü���� ã�� �� �����ϴ�");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //player = other.transform;
            detectable?.SetPlayerInRange(true);
            //if (M_Sight(player))
            //{
            //    detectable?.SetPlayerInRange(true);
            //    Debug.Log("�ν�");
            //}
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detectable?.SetPlayerInRange(false);
            //player = null;
        }
    }


    //void Update()
    //{
    //    if (player != null)
    //    {
    //        if (!M_Sight(player))
    //        {
    //            detectable?.SetPlayerInRange(false);
    //        }
    //    }
    //}

    //private bool M_Sight(Transform target)
    //{
    //    Vector2 eyePos = eyePosition.position;
    //    Vector2 targetPos = player.position;

    //    Vector2 flatTargetPos = new Vector2(targetPos.x, eyePos.y);
    //    Vector2 direction = (flatTargetPos - eyePos).normalized;
    //    float distance = Mathf.Abs(flatTargetPos.x - eyePos.x);

    //    RaycastHit2D hit = Physics2D.Raycast(eyePos, direction, distance, wall);

    //    if (hit.collider != null)
    //    {
    //        return false;
    //    }

    //    return true;
    //}
}
