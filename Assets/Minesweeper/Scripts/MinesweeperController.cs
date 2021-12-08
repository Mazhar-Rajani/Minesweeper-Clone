using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesweeperController : MonoBehaviour
{
    public static event Action OnFlagUpdate;
    public static event Action OnGameStart;
    public static event Action OnGameWon;
    public static event Action OnGameLost;

    [SerializeField] private MapGenerator mapGenerator = default;
    [SerializeField] private Sprite mineSprite = default;
    [SerializeField] private Sprite flagSprite = default;

    private bool inFlagState;
    private bool hasFoundMine;
    private bool isMiningComplete;
    private bool isGameOver;

    public int RemainingFlagCount { get; private set; }

    private void Awake()
    {
        inFlagState = false;

        MapGenerator.OnGenerateMap += OnGenerateMap;
        Cell.OnSelectCell += OnSelectCell;
    }

    private void OnDestroy()
    {
        MapGenerator.OnGenerateMap -= OnGenerateMap;
        Cell.OnSelectCell -= OnSelectCell;
    }

    private void OnGenerateMap()
    {
        hasFoundMine = false;
        isGameOver = false;
        RemainingFlagCount = mapGenerator.MineIndicies.Count;
        OnGameStart?.Invoke();
    }

    private void OnSelectCell(Cell cell, Cell.CellClickMode cellClickMode)
    {
        switch (cellClickMode)
        {
            case Cell.CellClickMode.Mine: HandleMineState(cell); break;
            case Cell.CellClickMode.Flag: HandleFlagState(cell); break;
        }
    }

    private void HandleMineState(Cell selectedCell)
    {
        if (selectedCell.IsFlagged || isGameOver)
            return;

        int mineCount = mapGenerator.GetCellMineCount(selectedCell);

        if (selectedCell.IsMine) HandleMineCell(selectedCell);
        else if (mineCount > 0) HandleNormalCell(selectedCell, mineCount);
        else HandleEmptyCell(selectedCell);

        CheckWinStatus();
    }

    private void HandleFlagState(Cell selectedCell)
    {
        if (selectedCell.IsFlagged)
        {
            RemainingFlagCount++;
            selectedCell.ToggleFlag(flagSprite);
        }
        else if (RemainingFlagCount > 0)
        {
            RemainingFlagCount--;
            selectedCell.ToggleFlag(flagSprite);
        }
        OnFlagUpdate?.Invoke();
    }

    private void HandleMineCell(Cell selectedCell)
    {
        hasFoundMine = true;
        foreach (Cell cell in mapGenerator.Cells)
        {
            if (cell.IsMine) cell.UpdateCell(string.Empty, mineSprite);
            else cell.DisableCell();
        }
        selectedCell.SetColor(Color.red);
    }

    private void HandleNormalCell(Cell selectedCell, int mineCount)
    {
        selectedCell.UpdateCell(mineCount.ToString(), null);
        isMiningComplete = !CanContinueMining();
    }

    private void HandleEmptyCell(Cell selectedCell)
    {
        Queue<Cell> emptyCells = new Queue<Cell>();
        emptyCells.Enqueue(selectedCell);

        while (emptyCells.Count != 0)
        {
            for (int i = 0; i < emptyCells.Count; i++)
            {
                Cell cell = emptyCells.Dequeue();
                cell.UpdateCell(null, null);

                List<Cell> adjCells = GetAdjacentCells(cell);
                foreach (Cell adjCell in adjCells)
                {
                    if (!adjCell.IsMine && adjCell.IsPending)
                    {
                        int mineCount = mapGenerator.GetCellMineCount(adjCell);
                        adjCell.UpdateCell(mineCount == 0 ? string.Empty : mineCount.ToString(), null);

                        if (mineCount == 0)
                        {
                            emptyCells.Enqueue(adjCell);
                        }
                    }
                }
            }
        }

        isMiningComplete = !CanContinueMining();
    }

    private List<Cell> GetAdjacentCells(Cell cell)
    {
        List<Cell> cells = new List<Cell>();

        int r = cell.Row;
        int c = cell.Column;

        Cell up = mapGenerator.GetCell(r, c + 1);
        Cell down = mapGenerator.GetCell(r, c - 1);
        Cell right = mapGenerator.GetCell(r + 1, c);
        Cell left = mapGenerator.GetCell(r - 1, c);
        Cell topRight = mapGenerator.GetCell(r + 1, c + 1);
        Cell topLeft = mapGenerator.GetCell(r + 1, c - 1);
        Cell bottomRight = mapGenerator.GetCell(r - 1, c + 1);
        Cell bottomLeft = mapGenerator.GetCell(r - 1, c - 1);

        if (up != null) cells.Add(up);
        if (down != null) cells.Add(down);
        if (right != null) cells.Add(right);
        if (left != null) cells.Add(left);
        if (topRight != null) cells.Add(topRight);
        if (topLeft != null) cells.Add(topLeft);
        if (bottomRight != null) cells.Add(bottomRight);
        if (bottomLeft != null) cells.Add(bottomLeft);

        return cells;
    }

    public void ToggleSelectionState()
    {
        inFlagState = !inFlagState;
    }

    private void CheckWinStatus()
    {
        isGameOver = hasFoundMine || isMiningComplete;

        if (hasFoundMine)
        {
            OnGameLost?.Invoke();
        }
        else if (isMiningComplete)
        {
            List<Cell> mineCells = mapGenerator.GetMineCells();
            for (int i = 0; i < mineCells.Count; i++)
            {
                mineCells[i].UpdateCell(string.Empty, mineSprite);
                mineCells[i].SetColor(Color.red);
            }
            OnGameWon?.Invoke();
        }
        else
        {
            isGameOver = false;
        }
    }

    private bool CanContinueMining()
    {
        foreach (Cell cell in mapGenerator.Cells)
        {
            if (!cell.IsMine && cell.IsPending)
                return true;
        }
        return false;
    }
}