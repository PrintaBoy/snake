using UnityEngine;

public class SnakeSegment : MonoBehaviour, ISpawnable
{
    private IGridTile parent;

    private float movementSpeed = 0.3f;
    private float doMoveTimer = 0f;
    private float doMoveTimerMax = 0.3f; // refactor to JSON, load from gamedata

    private Directions nextMoveDirection = Directions.North;
    private Directions currentMoveDirection = Directions.South;

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
        doMoveTimer += Time.deltaTime * movementSpeed;
        if (doMoveTimer >= doMoveTimerMax)
        {
            AttemptMovement();
        }
    }
    private void AttemptMovement()
    {
        doMoveTimer = 0f;
        IGridTile tileToMoveTo = parent.GetAdjecentTile(nextMoveDirection);
        GameObject tileSpawnedObject = tileToMoveTo.GetSpawnedObject();

        if (tileSpawnedObject == null)
        {
            parent.ClearChild();
            tileToMoveTo.BecomeParent(gameObject);
            DoMovement(tileToMoveTo.GetGridPosition());
        }
        else
        {
            if (tileSpawnedObject.TryGetComponent<ISpawnable>(out ISpawnable spawnedObject))
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
        AttemptMovement();
    }

    public void GetParent(GridTile snakeParentTile)
    {
        parent = snakeParentTile;
    }
}
