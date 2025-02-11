using UnityEngine;

public class SnakeSegment : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    private IGridTile previousParent;

    private float movementSpeed = 0.3f;
    private float doMoveTimer = 0f;
    private float doMoveTimerMax = 0.3f; // refactor to JSON, load from gamedata

    private Directions nextMoveDirection = Directions.North;
    private Directions currentMoveDirection = Directions.South;

    private void Start()
    {
        movementSpeed = GameData.gameData.snakeMovementSpeed;
    }

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(gameObject);        
    }

    public void Collision()
    {
        Debug.Log("Snake segment colision");
    }

    private void Update()
    {
        /*doMoveTimer += Time.deltaTime * movementSpeed;
        if (doMoveTimer >= doMoveTimerMax)
        {
            MoveHeadSegment();
        }*/
    }

    public void MoveTailSegment(IGridTile tileToMoveTo)
    {
        parent.ClearChild();
        tileToMoveTo.BecomeParent(gameObject);
        DoMovement(tileToMoveTo.gameObject.transform);        
    }

    public void MoveHeadSegment()
    {
        doMoveTimer = 0f;
        IGridTile tileToMoveTo = parent.GetAdjecentTile(nextMoveDirection);
        parent.ClearChild();
        tileToMoveTo.BecomeParent(gameObject);
        DoMovement(tileToMoveTo.gameObject.transform);
    }

    public void CheckForCollision(IGridTile tileToCheck)
    {       
        if (tileToCheck.GetSpawnedObject() != null)
        {
            if (tileToCheck.GetSpawnedObject().TryGetComponent<ISpawnable>(out ISpawnable spawnedObject)) // checks of the next tile is empty. If not, collision happens
            {
                spawnedObject.Collision();
            }
        }        
    }

    public void DoMovement(Transform gridToMoveTo)
    {
        gameObject.transform.position = gridToMoveTo.position;
        currentMoveDirection = nextMoveDirection;        
    }

    public void NewMoveDirection(Directions newDirection)
    {
        nextMoveDirection = newDirection;        
    }

    public Directions GetCurrentDirection()
    {
        return currentMoveDirection;
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
