using UnityEngine;

public class GridTileVisual : MonoBehaviour
{
    /// <summary>
    /// This script handles visual aspect of GridTile
    /// </summary>
    
    [SerializeField] private Material[] gridTileMaterials;
    [SerializeField] private MeshRenderer meshRenderer;
    private IGridTile gridTileScript;

    private void Awake()
    {
        gridTileScript = GetComponent<IGridTile>();
    }

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
        Vector2Int gridAddressRemainder = new Vector2Int(gridTileScript.GetGridTileAddress().x % 2, gridTileScript.GetGridTileAddress().y % 2);

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
