using UnityEngine;

public class Snake : MonoBehaviour, ISpawnable
{
    private float movementSpeed = 0f;
    private float doMoveTimer = 0f;
    private float doMoveTimerMax = 0.3f; // refactor to JSON, load from gamedata
    private GridTile parentTile;
    private Directions nextMoveDirection = Directions.North;

    private void Awake()
    {
        movementSpeed = GameData.gameData.snakeMovementSpeed;
    }

    private void Update()
    {
        doMoveTimer += Time.deltaTime * movementSpeed;
        if (doMoveTimer >= doMoveTimerMax)
        {
            AttemptMovement();                  
            doMoveTimer = 0f;
        }
    }

    private void AttemptMovement()
    {
        IGridTile tileToMoveTo = parentTile.GetAdjecentTile(nextMoveDirection);
        GameObject tileSpawnedObject = tileToMoveTo.GetSpawnedObject();

        if (tileSpawnedObject == null)
        {
            parentTile.spawnedObject = null;
            tileToMoveTo.BecomeParent(gameObject);
            DoMovement(tileToMoveTo.GetGridPosition());
        } else
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
    }

    public void Spawn()
    {

    }

    public void Collision()
    {

    }

    public void GetParent(GridTile snakeParentTile)
    {
        parentTile = snakeParentTile;
    }
}
