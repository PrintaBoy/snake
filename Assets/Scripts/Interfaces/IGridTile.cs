using UnityEngine;

public interface IGridTile
{
    void Generate();  
    bool HasObject();
    void SetupGridTile(Vector2Int gridTileAddress);
}
