using UnityEngine;

public class VisualMap : MonoBehaviour
{
    public Cell cellPrefab = default;
    public Transform content = default;
    public MapGenerator mapGenerator = default;

    private void Start()
    {
        UpdateVisualMap();
    }

    public void UpdateVisualMap()
    {
        foreach (Transform cell in content)
        {
            Destroy(cell.gameObject);
        }
        for (int r = 0; r < mapGenerator.rowCount; r++)
        {
            for (int c = 0; c < mapGenerator.columnCount; c++)
            {
                Cell cell = Instantiate(cellPrefab, content);
                cell.Setup(mapGenerator.cells[r, c]);
            }
        }
    }
}