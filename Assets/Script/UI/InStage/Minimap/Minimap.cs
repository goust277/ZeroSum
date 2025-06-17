using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Header("미니맵 UI")]
    [SerializeField] private RectTransform playerIcon;  // 미니맵에서 플레이어 위치를 표시할 아이콘
    [SerializeField] private RectTransform minimapBackground; // 미니맵 배경 이미지
    [SerializeField] private Transform player;  // 플레이어 Transform

    [Header("Minimap size")]
    [SerializeField] private float minimapSizeX = 300f;  // 미니맵의 너비
    [SerializeField] private float minimapSizeY = 568f;  // 미니맵의 높이
                                                         // 미니맵 배경 이동 시 필요한 계산 범위
    public float minBGY = -181f;  // 미니맵 배경 Y 최소값
    public float maxBGY = 180f;   // 미니맵 배경 Y 최대값


    [Header("VisualizeField size")]
    [SerializeField] private float visibleAreaHeight = 200f;  // 화면에 표시할 미니맵 영역 높이
    [SerializeField] private float iconYLimit = 20f;  // 미니맵 아이콘의 Y축 변화 범위

    // 아이콘 이동 범위 제한 (X, Y 값)
    [Header("ICon Field size")]
    [SerializeField] private float minX = -135f;
    [SerializeField] private float maxX = 133f;
    [SerializeField] private float minY = -83.2f;
    [SerializeField] private float maxY = 30f;

    // 아이콘 이동 범위 제한 (X, Y 값)
    [Header("Player Field size")]
    [SerializeField] private float minPlayerX = -12.5f;
    [SerializeField] private float maxPlayerX = 12.5f;
    [SerializeField] private float minPlayerY = -8.0f;
    [SerializeField] private float maxPlayerY = 37f;

    private void Update()
    {
        // 게임 월드 좌표 -> 미니맵 좌표로 변환
        Vector3 playerWorldPos = player.position;

        // 월드 좌표를 미니맵 좌표로 변환 (미니맵 크기 기준으로 비율 계산)
        float minimapX = Mathf.Clamp((playerWorldPos.x - minPlayerX) / (maxPlayerX - minPlayerX) * (maxX - minX) + minX, minX, maxX);
        float minimapY = Mathf.Clamp((playerWorldPos.y - minPlayerY) / (maxPlayerY - minPlayerY) * (maxY - minY) + minY, minY, maxY);

        // 미니맵 아이콘 위치 설정 (Y축 변화 제한)
        playerIcon.anchoredPosition = new Vector2(minimapX, minimapY);


        // 미니맵 배경을 플레이어의 y 위치에 맞게 이동
        UpdateMinimapBackground(playerWorldPos);
    }

    private void UpdateMinimapBackground(Vector3 playerWorldPos)
    {
        // 미니맵 배경의 Y축은 플레이어의 Y값에 따라 변동
        float minimapBackgroundY = Mathf.Lerp(maxBGY, minBGY, (playerWorldPos.y - minPlayerY) / (maxPlayerY - minPlayerY));

        // 배경의 y 위치 설정
        minimapBackground.anchoredPosition = new Vector2(minimapBackground.anchoredPosition.x, minimapBackgroundY);
    }
}
