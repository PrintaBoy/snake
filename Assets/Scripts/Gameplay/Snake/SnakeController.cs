using UnityEngine;
using System.Collections.Generic;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private List<SnakeSegment> snakeSegments;
    [SerializeField] private GameObject snakeSegmentPrefab;

    private void OnEnable()
    {
        GridController.OnGridGenerated += GridGenerated;
    }

    private void OnDisable()
    {
        GridController.OnGridGenerated -= GridGenerated;
    }

    private void GridGenerated() // reacts to event Grid.OnGridGenerated
    {
        GenerateSnakeSegment(); // spawns the first segment of a snake
    }

    public void ChangeDirection(Directions newDirection) // receives from MoveCommand command to change direction of snake
    {
        snakeSegments[0].NewMoveDirection(newDirection);

        // here give commands to the tail as well, once it's spawned
    }

    private void GenerateSnakeSegment()
    {
        IGridTile emptyTile = GridController.instance.GetEmptyTile();        
        
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
