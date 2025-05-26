using UnityEngine;

public class Detection : MonoBehaviour
{
    private IDetectable detectable;

    //[SerializeField] private Transform eyePosition; // 몬스터의 눈 위치
    //[SerializeField] private LayerMask wall; // 벽 레이어

    //private Transform player;

    void Start()
    {
        detectable = GetComponentInParent<IDetectable>();

        if (detectable == null)
        {
            Debug.LogError("부모 객체에서 찾을 수 없습니다");
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
            //    Debug.Log("인식");
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
