using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public enum PipeType { Straight,SideAway, Corner, TShape, Cross, 
        LeftDownCorner, LeftUpCorner, RightDownCorner
            , Right, Left }
    public PipeType type;
    public bool[] connections = new bool[4]; // [��, ��, ��, ��]
    private int rotationStep;

    public bool isStart;
    public bool isEnd = false;

    [SerializeField] private ChangeLink[] changeLinks;
    void Start()
    {
        UpdateConnections();
    }

    // ������ ȸ�� ���� �Լ�
    public void RotatePipe()
    {
        rotationStep = (rotationStep + 1) % 4;
        transform.Rotate(0, 0, 90);
        UpdateConnections();

        FindObjectOfType<GameManager>().OnPipeRotated();
    }

    // ȸ���� ���� ���� ���� ������Ʈ
    private void UpdateConnections()
    {
        bool[] baseConnections = GetBaseConnections();
        connections = RotateArray(baseConnections, rotationStep);
    }

    // ������ Ÿ�Ժ� �⺻ ���� ����
    private bool[] GetBaseConnections()
    {
        switch (type)
        {
            case PipeType.Straight: return new[] { true, false, true, false };
            case PipeType.SideAway: return new[] { false, true, false, true };
            case PipeType.Corner: return new[] { true, true, false, false };
            case PipeType.TShape: return new[] { true, true, true, false };
            case PipeType.Cross: return new[] { true, true, true, true };
            case PipeType.LeftDownCorner: return new[] { false, false, true, true };
            case PipeType.LeftUpCorner:return new[] { true, false, false, true };
            case PipeType.RightDownCorner: return new[] { false, true, true, false };
            case PipeType.Right: return new[] { false, true, false, false };
            case PipeType.Left: return new[] { false, false, false, true };
            default: return new bool[4];
        }
    }

    // �迭 ȸ�� ��ƿ��Ƽ �Լ�
    private bool[] RotateArray(bool[] arr, int step)
    {
        bool[] result = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            result[(i + step) % 4] = arr[i];
        }
        return result;
    }


    public void SetColor()
    {
        foreach(var links in changeLinks)
        {
            links.SetSprite();
        }
    }

    public void ResetColor()
    {
        {
            foreach (var links in changeLinks)
            {
                links.ResetSprite();
            }
        }
    }
}
