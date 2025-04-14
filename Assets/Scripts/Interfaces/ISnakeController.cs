using UnityEngine;

public interface ISnakeController
{
    void ChangeSnakeDirection(Directions newDirection);
    void MoveSnake(Directions moveDirection);
    IGridTile GetSnakeHeadTile();
}
