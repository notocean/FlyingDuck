using UnityEngine;
using UnityEngine.UI;

public class GridAutoSizer : MonoBehaviour
{
    GridLayoutGroup gridLayout;

    private void Awake() {
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    private void Start() {
        AdjustCellSize();
    }

    private void AdjustCellSize() {
        RectTransform rectTransform = GetComponent<RectTransform>();

        float width = rectTransform.rect.width;
        float cellSize = (width - gridLayout.spacing.x * (gridLayout.constraintCount - 1)) / gridLayout.constraintCount;

        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
