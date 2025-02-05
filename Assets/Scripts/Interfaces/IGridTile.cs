using UnityEngine;

public interface IGridTile
{
    void GenerateGridTile();
    void GenerateObstacle(GameObject obstacleToGenerate);
    void GetAdjecentTiles();

    bool HasObject();
    void SetupGridTile(Vector2Int gridTileAddress);
}
