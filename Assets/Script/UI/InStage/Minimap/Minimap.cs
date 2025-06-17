using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Header("�̴ϸ� UI")]
    [SerializeField] private RectTransform playerIcon;  // �̴ϸʿ��� �÷��̾� ��ġ�� ǥ���� ������
    [SerializeField] private RectTransform minimapBackground; // �̴ϸ� ��� �̹���
    [SerializeField] private Transform player;  // �÷��̾� Transform

    [Header("Minimap size")]
    [SerializeField] private float minimapSizeX = 300f;  // �̴ϸ��� �ʺ�
    [SerializeField] private float minimapSizeY = 568f;  // �̴ϸ��� ����
                                                         // �̴ϸ� ��� �̵� �� �ʿ��� ��� ����
    public float minBGY = -181f;  // �̴ϸ� ��� Y �ּҰ�
    public float maxBGY = 180f;   // �̴ϸ� ��� Y �ִ밪


    [Header("VisualizeField size")]
    [SerializeField] private float visibleAreaHeight = 200f;  // ȭ�鿡 ǥ���� �̴ϸ� ���� ����
    [SerializeField] private float iconYLimit = 20f;  // �̴ϸ� �������� Y�� ��ȭ ����

    // ������ �̵� ���� ���� (X, Y ��)
    [Header("ICon Field size")]
    [SerializeField] private float minX = -135f;
    [SerializeField] private float maxX = 133f;
    [SerializeField] private float minY = -83.2f;
    [SerializeField] private float maxY = 30f;

    // ������ �̵� ���� ���� (X, Y ��)
    [Header("Player Field size")]
    [SerializeField] private float minPlayerX = -12.5f;
    [SerializeField] private float maxPlayerX = 12.5f;
    [SerializeField] private float minPlayerY = -8.0f;
    [SerializeField] private float maxPlayerY = 37f;

    private void Update()
    {
        // ���� ���� ��ǥ -> �̴ϸ� ��ǥ�� ��ȯ
        Vector3 playerWorldPos = player.position;

        // ���� ��ǥ�� �̴ϸ� ��ǥ�� ��ȯ (�̴ϸ� ũ�� �������� ���� ���)
        float minimapX = Mathf.Clamp((playerWorldPos.x - minPlayerX) / (maxPlayerX - minPlayerX) * (maxX - minX) + minX, minX, maxX);
        float minimapY = Mathf.Clamp((playerWorldPos.y - minPlayerY) / (maxPlayerY - minPlayerY) * (maxY - minY) + minY, minY, maxY);

        // �̴ϸ� ������ ��ġ ���� (Y�� ��ȭ ����)
        playerIcon.anchoredPosition = new Vector2(minimapX, minimapY);


        // �̴ϸ� ����� �÷��̾��� y ��ġ�� �°� �̵�
        UpdateMinimapBackground(playerWorldPos);
    }

    private void UpdateMinimapBackground(Vector3 playerWorldPos)
    {
        // �̴ϸ� ����� Y���� �÷��̾��� Y���� ���� ����
        float minimapBackgroundY = Mathf.Lerp(maxBGY, minBGY, (playerWorldPos.y - minPlayerY) / (maxPlayerY - minPlayerY));

        // ����� y ��ġ ����
        minimapBackground.anchoredPosition = new Vector2(minimapBackground.anchoredPosition.x, minimapBackgroundY);
    }
}
