using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image cellImage = default;
    [SerializeField] private Color hoverColor = default;

    private Color normalColor;

    private void Awake()
    {
        normalColor = cellImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cellImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cellImage.color = normalColor;
    }
}