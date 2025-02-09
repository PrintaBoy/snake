using UnityEngine;

public interface IGridTile
{
    GameObject gameObject { get; }
    void GenerateGridTile();
    void GenerateObstacle(GameObject obstacleToGenerate);    
    IGridTile GetAdjecentTile(Directions direction);
    void GetAdjecentTiles();   

    bool HasObject();
    void SetupGridTile(Vector2Int gridTileAddress);

    void BecomeParent(GameObject child);
    void ClearChild();

    GameObject GetSpawnedObject();

    Transform GetGridPosition();
}
