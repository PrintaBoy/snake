using UnityEngine;

public interface ISpawnable
{
    GameObject gameObject { get; }
    void SetupSpawnable(IGridTile parentTile);
    void Collision();
    void ParentToTile(GridTile parentTile);
}
