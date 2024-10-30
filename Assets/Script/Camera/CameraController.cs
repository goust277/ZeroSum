using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 cameraPosition = new Vector3(0, 0, -10); // 카메라 오프셋
    [SerializeField] private float smoothSpeed = 0.15f; // 카메라 추격 속도
    [SerializeField] private Vector2 center; // 맵의 중앙
    [SerializeField] private Vector2 mapSize; // 맵 크기

    private float height;
    private float width;

    private GameObject player;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.transform.position + cameraPosition;

        float lx = mapSize.x - width;
        float ly = mapSize.y - height;

        float clampX = Mathf.Clamp(desiredPosition.x, -lx + center.x, lx + center.x);
        float clampY = Mathf.Clamp(desiredPosition.y, -ly + center.y, ly + center.y);

        // 제한된 위치로 desiredPosition 업데이트
        desiredPosition = new Vector3(clampX, clampY, desiredPosition.z);

        // 부드러운 이동 적용 (선택적)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
