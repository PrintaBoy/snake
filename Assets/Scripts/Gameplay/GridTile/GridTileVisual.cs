using UnityEngine;

public class GridTileVisual : MonoBehaviour
{
    [SerializeField] private Material[] gridTileMaterials;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GridTile gridTileScript;

    private void OnEnable()
    {
        GridController.OnGridGenerated += GridGenerated;
    }

    private void OnDisable()
    {
        GridController.OnGridGenerated -= GridGenerated;
    }

    private void GridGenerated()
    {
        ChangeGridMaterial();
    }

    private void ChangeGridMaterial()
    {
        Vector2Int gridAddressRemainder = new Vector2Int(gridTileScript.gridAddress.x % 2, gridTileScript.gridAddress.y % 2);

        if (gridAddressRemainder == new Vector2Int(0, 0) || gridAddressRemainder == new Vector2Int(1, 1))
        {
            meshRenderer.material = gridTileMaterials[0];
        }

        if (gridAddressRemainder == new Vector2Int(1, 0) || gridAddressRemainder == new Vector2Int(0, 1))
        {
            meshRenderer.material = gridTileMaterials[1];
        }
    }
}
