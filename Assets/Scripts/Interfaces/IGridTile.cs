using UnityEngine;

public interface IGridTile
{
    GameObject gameObject { get; }
    void GenerateGridTile();      
    IGridTile GetAdjecentTile(Directions direction);
    void MapAdjecentTiles();   

    bool HasObject();
    void SetupGridTile(Vector2Int gridTileAddress);

    void BecomeParent(ISpawnable child);
    void ClearChild();

    Vector2Int GetGridTileAddress();

    ISpawnable GetSpawnedObject();
}
