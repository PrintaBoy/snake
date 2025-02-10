using UnityEngine;
using System.Collections.Generic;
using System;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private List<SnakeSegment> snakeSegments;
    [SerializeField] private GameObject snakeSegmentPrefab;

    public static event Action OnSnakeSpawned;

    private void OnEnable()
    {
        GridController.OnGridGenerated += GridGenerated;
        Consumable.OnAppleConsumed += GenerateSnakeSegment;
    }

    private void OnDisable()
    {
        GridController.OnGridGenerated -= GridGenerated;
        Consumable.OnAppleConsumed -= GenerateSnakeSegment;
    }

    private void GridGenerated() // reacts to event Grid.OnGridGenerated
    {
        GenerateSnakeSegment(); // spawns the first segment of a snake
    }

    public void MoveSnake(Directions newDirection) // receives from MoveCommand command to change direction of snake
    {

        snakeSegments[0].CheckForCollision(snakeSegments[0].GetParent().GetAdjecentTile(newDirection));

        for (int i = 1; i < snakeSegments.Count; i++) // move tail first
        {
            snakeSegments[i].NewMoveDirection(snakeSegments[i - 1].GetCurrentDirection());
            snakeSegments[i].MoveTailSegment();            
        }

        snakeSegments[0].NewMoveDirection(newDirection); // move head later
        snakeSegments[0].MoveHeadSegment();

    }

    private void GenerateSnakeSegment()
    {
        IGridTile emptyTile = null;

        if (snakeSegments.Count == 0) // gets tile for spawning head
        {
            emptyTile = GridController.instance.GetEmptyTile();
            OnSnakeSpawned?.Invoke();
        }
        else // gets tile for spawning snake segment
        {
            // get tile last snake segment is on            
            emptyTile = snakeSegments[snakeSegments.Count - 1].GetPreviousParent();
            // get previous tile
            Debug.Log(emptyTile);
        }
        
        GameObject generatedSnakeSegment = Instantiate(snakeSegmentPrefab, emptyTile.gameObject.transform.position, emptyTile.gameObject.transform.rotation);
        
        if (generatedSnakeSegment.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) //setup spawned snake segment
        {
            spawnable.SetupSpawnable(emptyTile);            
        }

        if (generatedSnakeSegment.TryGetComponent<SnakeSegment>(out SnakeSegment snakeSegment)) // add generated SnakeSegment to list
        {               
            snakeSegments.Add(snakeSegment);            
        }
    }
}
