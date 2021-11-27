using UnityEngine;
using Drawing;
using Unity.Mathematics;
using System;

public class MapGenerator : MonoBehaviour
{
    public int rowCount = default;
    public int columnCount = default;
    public int mineCount = default;

    [Header("Debug")]
    public int[] mineIndicies;
    public int[,] cells;

    private void Awake()
    {
        Generate();
    }

    private void Update()
    {
        DebugDraw();
    }

    public void Generate()
    {
        cells = new int[rowCount, columnCount];
        mineIndicies = new int[mineCount];
        CreateMines();
        CreateCells();
    }

    private void CreateMines()
    {
        for (int m = 0; m < mineCount; m++)
        {
            int i = UnityEngine.Random.Range(0, rowCount * columnCount);
            mineIndicies[m] = i;
            int r = Mathf.FloorToInt(i % rowCount);
            int c = Mathf.FloorToInt(i / rowCount);
            cells[r, c] = -1;
        }
    }

    private void CreateCells()
    {
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                cells[r, c] = GetValue(r, c);
            }
        }
    }

    private int GetValue(int r, int c)
    {
        if (cells[r, c] != -1)
        {
            int t = 0;

            t += CheckValue(r, c + 1);
            t += CheckValue(r, c - 1);
            t += CheckValue(r + 1, c);
            t += CheckValue(r - 1, c);
            t += CheckValue(r + 1, c + 1);
            t += CheckValue(r - 1, c - 1);
            t += CheckValue(r + 1, c - 1);
            t += CheckValue(r - 1, c + 1);

            return t;
        }
        return -1;
    }

    private int CheckValue(int r, int c)
    {
        if (r >= 0 && r < rowCount && c >= 0 && c < columnCount)
        {
            if (cells[r, c] == -1)
            {
                return 1;
            }
        }
        return 0;

    }

    private void DebugDraw()
    {
        if (cells == null)
            return;

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                float3 pos = new float3(r, c, 0);
                Draw.CircleXY(pos, 0.5f, Color.red);
                Draw.Label2D(pos, cells[r, c].ToString(), 40, LabelAlignment.Center, Color.white);
            }
        }
    }
}