using UnityEngine;

public interface ISpawnable
{
    GameObject gameObject { get; }
    void SetupSpawnable(IGridTile parentTile);
    void Collision(ISpawnable collisionObject);
    void ParentToTile(GridTile parentTile);
    Vector2Int GetParentGridAddress();
}
