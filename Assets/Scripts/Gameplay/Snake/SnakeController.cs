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
        IGridTile adjecentTileInDirection = snakeSegments[0].GetParent().GetAdjecentTile(newDirection);

        snakeSegments[0].CheckForCollision(adjecentTileInDirection);        
        snakeSegments[0].MoveSnakeSegment(adjecentTileInDirection);

        for (int i = 1; i < snakeSegments.Count; i++)
        {            
            snakeSegments[i].MoveSnakeSegment(snakeSegments[i - 1].GetPreviousParent());            
        }
    }

    private void GenerateSnakeSegment()
    {
        IGridTile emptyTile;

        if (snakeSegments.Count == 0) // gets tile for spawning head
        {
            emptyTile = GridController.instance.GetEmptyTile();
            OnSnakeSpawned?.Invoke();
        }
        else // gets tile for spawning snake segment
        {            
            emptyTile = snakeSegments[snakeSegments.Count - 1].GetPreviousParent();
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
