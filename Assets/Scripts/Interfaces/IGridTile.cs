using UnityEngine;

public interface IGridTile
{
    void GenerateGridTile();
    void GenerateObstacle(GameObject obstacleToGenerate);
    void GenerateSnake(GameObject snakeToGenerate);
    void GetAdjecentTiles();   

    bool HasObject();
    void SetupGridTile(Vector2Int gridTileAddress);

    void BecomeParent(GameObject child);

    GameObject GetSpawnedObject();

    Transform GetGridPosition();
}
