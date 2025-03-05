using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSavePoint : MonoBehaviour
{
    [SerializeField] private int stagePointID = 0;
    private bool isCollision = false;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && !isCollision) // 충돌한 오브젝트의 Collider 비교
        {
            GameStateManager.Instance.SetStagePoint(stagePointID);
            isCollision = true;
            Destroy(gameObject, 0.5f); // 1초 후 삭제
        
            
        }
    }
}
