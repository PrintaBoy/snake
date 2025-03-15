using UnityEngine;
using System;

public class SnakeSegment : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    private IGridTile previousParent;
    [HideInInspector] public int snakeSegmentListIndex { get; private set; } // index of this snake segment in list in SnakeController
    public static event Action OnSnakeSegmentSetup;
    public static event Action OnSnakeSegmentCollision;
    public static event Action OnSnakeSegmentDestroy;

    private void OnEnable()
    {
        SnakeController.OnSnakeCollision += Collision;
    }

    private void OnDisable()
    {
        SnakeController.OnSnakeCollision -= Collision;
    }

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;        
        parent.BecomeParent(this);
        gameObject.transform.position = parentTile.gameObject.transform.position;
        gameObject.transform.rotation = parentTile.gameObject.transform.rotation;
        OnSnakeSegmentSetup?.Invoke();
    }

    public void Collision(ISpawnable collisionObject)
    {
        if (collisionObject == this)
        {
            OnSnakeSegmentCollision?.Invoke();
            // disable snake segment the snake bit into
        }        
    }

    public void SetListIndex(int listIndex)
    {
        snakeSegmentListIndex = listIndex;
        OnSnakeSegmentSetup?.Invoke();
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

    public Vector2Int GetParentGridAddress()
    {
        return parent.GetGridTileAddress();
    }

    public IGridTile GetPreviousParent()
    {
        return previousParent;
    }

    public Directions GetLastMoveDirection()
    {
        Directions lastMoveDirection = Directions.West;
        // compare parent and previous parent to get direction this segment moved in 
        if (parent.GetGridTileAddress().x != previousParent.GetGridTileAddress().x) // if the X coordinate is not the same it means the snake segment arrived from west or east
        {
            if (parent.GetGridTileAddress().x > previousParent.GetGridTileAddress().x)
            {
                // snake arrived from east
                lastMoveDirection = Directions.West;
            } else
            {
                // snake arrived from west
                lastMoveDirection = Directions.East;
            }
        }
        else // if the X coordinate is the same it means the snake segment arrived from north or south
        if (parent.GetGridTileAddress().y > previousParent.GetGridTileAddress().y)
        {
            // snake arrived from north
            lastMoveDirection = Directions.South;
        } else
        {
            // snake arrived from south
            lastMoveDirection = Directions.North;
        }       

        return lastMoveDirection;
    }

    public void DeleteSnakeSegment()
    {
        parent.ClearChild();
        Destroy(gameObject);
        OnSnakeSegmentDestroy?.Invoke();    
    }
}
