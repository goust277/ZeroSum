using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public PathValidator pathValidator;

    // ������ ȸ�� �� ȣ�� (��: ������ Ŭ��, UI ��ư ���)
    public void OnPipeRotated()
    {
        Vector2Int start = gridManager.FindStartPosition();
        Debug.Log($"GetConnectedPath ȣ��: ���� ��ġ ({start.x}, {start.y})");
        if (gridManager.grid == null)
        {
            Debug.LogError("�׸��尡 null�Դϴ�.");
        }
        List<Pipe> connectedPipes = pathValidator.GetConnectedPath(start, gridManager.grid);

        gridManager.ResetAllPipeColors(); // ��� ������ ���� �ʱ�ȭ

        foreach (Pipe pipe in connectedPipes)
            pipe.SetColor();

        // �ʿ��ϴٸ� ���⼭ �������� �����ߴ����� �߰��� üũ ����
        Vector2Int end = gridManager.FindEndPosition();
        //if (connectedPipes.Contains(gridManager.grid[end.x, end.y])) { ... }
        Pipe endPipe = gridManager.grid[end.x, end.y];
        if (connectedPipes.Contains(endPipe))
        {
            Debug.Log("���� Ŭ����! �������� �����");
            
        }
    }
}
