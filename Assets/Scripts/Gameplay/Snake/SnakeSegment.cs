using UnityEngine;
using System;

public class SnakeSegment : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    private IGridTile previousParent;
    [HideInInspector] public int snakeSegmentListIndex { get; private set; } // index of this snake segment in list in SnakeController
    public static event Action OnSnakeSegmentSetup;
    public static event Action OnObstacleCollision;

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
            OnObstacleCollision?.Invoke();
            // disable snake segment the snake bit into
        }        
    }

    public void SetListIndex(int listIndex)
    {
        snakeSegmentListIndex = listIndex;
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
        Debug.Log(parent + " --- " + previousParent);
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
