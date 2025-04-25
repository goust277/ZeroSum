using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathValidator
{
    public List<Pipe> GetConnectedPath(Vector2Int start, Pipe[,] grid)
    {
        List<Pipe> connectedPipes = new List<Pipe>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            Pipe currentPipe = grid[current.x, current.y];
            connectedPipes.Add(currentPipe);

            for (int dir = 0; dir < 4; dir++)
            {
                Vector2Int neighbor = GetNeighbor(current, dir);
                if (IsConnected(current, neighbor, grid) && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }
        return connectedPipes;
    }

    private bool IsConnected(Vector2Int from, Vector2Int to, Pipe[,] grid)
    {
        int sizeX = grid.GetLength(0);
        int sizeY = grid.GetLength(1);
        if (to.x < 0 || to.x >= sizeX || to.y < 0 || to.y >= sizeY)
            return false;

        Pipe fromPipe = grid[from.x, from.y];
        Pipe toPipe = grid[to.x, to.y];
        if (fromPipe == null || toPipe == null)
            return false;

        int dir = GetDirection(from, to);
        int reverseDir = (dir + 2) % 4;
        return fromPipe.connections[dir] && toPipe.connections[reverseDir];
    }

    private int GetDirection(Vector2Int from, Vector2Int to)
    {
        Vector2Int delta = to - from;
        if (delta.y > 0) return 0; // ╩С
        if (delta.x > 0) return 1; // ©Л
        if (delta.y < 0) return 2; // го
        return 3; // аб
    }

    private Vector2Int GetNeighbor(Vector2Int pos, int dir)
    {
        switch (dir)
        {
            case 0: return new Vector2Int(pos.x, pos.y + 1);
            case 1: return new Vector2Int(pos.x + 1, pos.y);
            case 2: return new Vector2Int(pos.x, pos.y - 1);
            default: return new Vector2Int(pos.x - 1, pos.y);
        }
    }
}
