using UnityEngine;

public class SnakeSegment : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    private IGridTile previousParent;

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(this);
        gameObject.transform.position = parentTile.gameObject.transform.position;
        gameObject.transform.rotation = parentTile.gameObject.transform.rotation;
    }

    public void Collision(ISpawnable collisionObject)
    {
        Debug.Log("Snake segment colision");
    }

    public void MoveSnakeSegment(IGridTile tileToMoveTo)
    {    
        parent.ClearChild();
        tileToMoveTo.BecomeParent(this);        
        gameObject.transform.position = tileToMoveTo.gameObject.transform.position;
    } 

    public void ParentToTile(GridTile snakeParentTile)
    {
        previousParent = parent;
        parent = snakeParentTile;
    }

    public IGridTile GetParent()
    {
        return parent;
    }

    public IGridTile GetPreviousParent()
    {
        return previousParent;
    }
}
