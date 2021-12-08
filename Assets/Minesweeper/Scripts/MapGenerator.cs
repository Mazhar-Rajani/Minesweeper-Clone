using System;
using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public static event Action OnGenerateMap;

    [SerializeField] private int rowCount = default;
    [SerializeField] private int columnCount = default;
    [SerializeField] private int mineCount = default;
    [SerializeField] private Cell cellPrefab = default;
    [SerializeField] private Transform content = default;

    public List<int> MineIndicies { get; private set; }
    public Cell[,] Cells { get; private set; }

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        ClearCells();
        CreateMines();
        CreateCells();
        OnGenerateMap?.Invoke();
    }

    private void ClearCells()
    {
        if (Cells != null)
        {
            foreach (Cell cell in Cells)
            {
                Destroy(cell.gameObject);
            }
        }
    }

    private void CreateMines()
    {
        int cellCount = rowCount * columnCount;

        List<int> allIndicies = new List<int>();
        for (int i = 0; i < cellCount; i++)
        {
            allIndicies.Add(i);
        }

        MineIndicies = new List<int>();
        for (int i = 0; i < mineCount; i++)
        {
            int m = allIndicies[UnityEngine.Random.Range(0, allIndicies.Count)];
            MineIndicies.Add(m);
            allIndicies.Remove(m);
        }
    }

    private void CreateCells()
    {
        Cells = new Cell[rowCount, columnCount];
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                Cell cell = Instantiate(cellPrefab, content);
                int cellIndex = GetCellIndex(r, c);
                cell.Setup(cellIndex, r, c, MineIndicies.Contains(cellIndex));
                Cells[r, c] = cell;
            }
        }
    }

    public Cell GetCell(int r, int c)
    {
        if (r >= 0 && r < rowCount && c >= 0 && c < columnCount)
        {
            return Cells[r, c];
        }
        return null;
    }

    public List<Cell> GetMineCells()
    {
        List<Cell> mineCells = new List<Cell>();
        foreach (Cell cell in Cells)
        {
            if (cell.IsMine)
            {
                mineCells.Add(cell);
            }
        }
        return mineCells;
    }

    public int GetCellMineCount(Cell cell)
    {
        int t = 0;
        int r = cell.Row;
        int c = cell.Column;
        t += CheckMine(r, c + 1);
        t += CheckMine(r, c - 1);
        t += CheckMine(r + 1, c);
        t += CheckMine(r - 1, c);
        t += CheckMine(r + 1, c + 1);
        t += CheckMine(r - 1, c - 1);
        t += CheckMine(r + 1, c - 1);
        t += CheckMine(r - 1, c + 1);
        return t;
    }

    private int CheckMine(int r, int c)
    {
        Cell cell = GetCell(r, c);
        if (cell != null) return Convert.ToInt32(cell.IsMine);
        return 0;
    }

    public int GetCellIndex(int r, int c)
    {
        return r * columnCount + c;
    }

    public Vector2Int GetRowCol(Cell cell)
    {
        return new Vector2Int(Mathf.FloorToInt(cell.Index % rowCount), Mathf.FloorToInt(cell.Index / rowCount));
    }
}