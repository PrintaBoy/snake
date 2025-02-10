using UnityEngine;

public class Obstacle : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    public void SetupSpawnable(IGridTile parentTile)
    {
    }

    public void Collision()
    {
        Debug.Log("Collision!");
    }

    public void ParentToTile(GridTile snakeParentTile)
    {
        parent = snakeParentTile;
    }
}
