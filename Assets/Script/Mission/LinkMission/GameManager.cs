using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public PathValidator pathValidator;

    // 파이프 회전 후 호출 (예: 파이프 클릭, UI 버튼 등에서)
    public void OnPipeRotated()
    {
        Vector2Int start = gridManager.FindStartPosition();
        Debug.Log($"GetConnectedPath 호출: 시작 위치 ({start.x}, {start.y})");
        if (gridManager.grid == null)
        {
            Debug.LogError("그리드가 null입니다.");
        }
        List<Pipe> connectedPipes = pathValidator.GetConnectedPath(start, gridManager.grid);

        gridManager.ResetAllPipeColors(); // 모든 파이프 색상 초기화

        foreach (Pipe pipe in connectedPipes)
            pipe.SetColor();

        // 필요하다면 여기서 끝점까지 도달했는지도 추가로 체크 가능
        Vector2Int end = gridManager.FindEndPosition();
        //if (connectedPipes.Contains(gridManager.grid[end.x, end.y])) { ... }
        Pipe endPipe = gridManager.grid[end.x, end.y];
        if (connectedPipes.Contains(endPipe))
        {
            Debug.Log("퍼즐 클리어! 끝점까지 연결됨");
            
        }
    }
}
