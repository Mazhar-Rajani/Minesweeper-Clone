using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public static event Action<Cell, CellClickMode> OnSelectCell;

    [SerializeField] private TMP_Text mineCountText = default;
    [SerializeField] private Image outerImg = default;
    [SerializeField] private Image innerImg = default;
    [SerializeField] private Button btn = default;
    [SerializeField] private CellHover cellHover = default;
    [SerializeField] private Shadow shadow = default;
    [SerializeField] private Color clickedColor = default;

    public int Index { get; private set; }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public bool IsMine { get; private set; }
    public bool IsFlagged { get; private set; }
    public bool IsPending { get; private set; }

    private void Awake()
    {
        mineCountText.enabled = false;
        DisableInnerElements();
    }

    public void Setup(int i, int r, int c, bool isMine)
    {
        Index = i;
        Row = r;
        Column = c;
        IsMine = isMine;
        IsFlagged = false;
        IsPending = true;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    private void DisableInnerElements()
    {
        innerImg.enabled = false;
        outerImg.enabled = true;
    }

    public void UpdateCell(string text = null, Sprite sprite = null)
    {
        mineCountText.enabled = !string.IsNullOrEmpty(text);
        innerImg.enabled = sprite != null;

        mineCountText.text = text;
        innerImg.sprite = sprite;

        cellHover.enabled = false;
        shadow.enabled = false;
        outerImg.color = clickedColor;
        btn.enabled = false;

        IsPending = false;
    }

    public void ToggleFlag(Sprite flagSprite)
    {
        DisableInnerElements();
        IsFlagged = !IsFlagged;
        innerImg.enabled = IsFlagged;
        innerImg.sprite = flagSprite;
    }

    public void DisableCell()
    {
        DisableInnerElements();
        innerImg.enabled = IsFlagged;
        btn.enabled = false;
        cellHover.enabled = false;
    }

    public void SetColor(Color color)
    {
        outerImg.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnSelectCell?.Invoke(this, CellClickMode.Mine);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnSelectCell?.Invoke(this, CellClickMode.Flag);
        }
    }

    public void ShowMineCount(string text)
    {
        mineCountText.enabled = true;
        mineCountText.text = text;
    }

    public enum CellClickMode
    {
        Mine,
        Flag
    }
}