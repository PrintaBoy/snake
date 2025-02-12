using UnityEngine;

public class SnakeSegment : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    private IGridTile previousParent;

    private float movementSpeed = 0.3f; 
    private float doMoveTimer = 0f;
    private float doMoveTimerMax = 0.3f; // refactor to JSON, load from gamedata

    private void Start()
    {
        movementSpeed = GameData.gameData.snakeMovementSpeed;
    }

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(this);
        gameObject.transform.position = parentTile.gameObject.transform.position;
        gameObject.transform.rotation = parentTile.gameObject.transform.rotation;
    }

    public void Collision()
    {
        Debug.Log("Snake segment colision");
    }

    private void Update() // mve timer to snakeController.cs
    {
        doMoveTimer += Time.deltaTime * movementSpeed;
        if (doMoveTimer >= doMoveTimerMax)
        {
            //MoveSnakeSegment();
        }
    }

    public void MoveSnakeSegment(IGridTile tileToMoveTo)
    {
        doMoveTimer = 0f;       
        parent.ClearChild();
        tileToMoveTo.BecomeParent(this);        
        gameObject.transform.position = tileToMoveTo.gameObject.transform.position;
    }

    public void CheckForCollision(IGridTile tileToCheck)
    {
        ISpawnable tileToCheckSpawnedObject = tileToCheck.GetSpawnedObject();
        if (tileToCheckSpawnedObject != null)
        {
            tileToCheckSpawnedObject.Collision();
        }        
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
