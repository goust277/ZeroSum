using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 8;
    public int gridHeight = 8;
    public float cellSize = 1.0f;
    public Pipe[,] grid;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new Pipe[gridWidth, gridHeight];
        // �׸��忡 �ִ� ��� ������ ���� ����
        Pipe[] allPipes = FindObjectsOfType<Pipe>();
        foreach (Pipe pipe in allPipes)
        {
            Vector2Int gridPos = WorldToGridPosition(pipe.transform.position);
            if (IsValidPosition(gridPos))
            {
                grid[gridPos.x, gridPos.y] = pipe;
            }
        }
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        // �׸��� ������Ʈ�� ��ġ�� ȸ���� ����� ���� ��ǥ�� ��ȯ
        Vector3 localPos = transform.InverseTransformPoint(worldPos);

        // �� ũ��� ������ �ε��� ���
        int gridX = Mathf.FloorToInt(localPos.x / cellSize);
        int gridY = Mathf.FloorToInt(localPos.y / cellSize);

        return new Vector2Int(gridX, gridY);
    }

    public Vector2Int FindStartPosition()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                if (grid[x, y] != null && grid[x, y].isStart)
                    return new Vector2Int(x, y);
        return new Vector2Int(-1, -1);
    }

    public Vector2Int FindEndPosition()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                if (grid[x, y] != null && grid[x, y].isEnd)
                    return new Vector2Int(x, y);
        return new Vector2Int(-1, -1);
    }

    public void ResetAllPipeColors()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                if (grid[x, y] != null)
                    grid[x, y].ResetColor();
    }

    bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }
}