using UnityEngine;

public interface IGridTile
{
    void GenerateGridTile();
    void GenerateObstacle(GameObject obstacleToGenerate);    

    bool HasObject();
    void SetupGridTile(Vector2Int gridTileAddress);
}
