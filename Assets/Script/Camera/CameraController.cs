using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 cameraPosition = new Vector3(0, 0, -10); // ī�޶� ������
    [SerializeField] private float smoothSpeed = 0.15f; // ī�޶� �߰� �ӵ�
    [SerializeField] private Vector2 center; // ���� �߾�
    [SerializeField] private Vector2 mapSize; // �� ũ��

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

        // ���ѵ� ��ġ�� desiredPosition ������Ʈ
        desiredPosition = new Vector3(clampX, clampY, desiredPosition.z);

        // �ε巯�� �̵� ���� (������)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
