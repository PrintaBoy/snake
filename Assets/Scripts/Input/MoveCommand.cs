public class MoveCommand : ICommand
{
    private SnakeController _snake;
    private Directions _moveDirection;

    public MoveCommand(SnakeController snake, Directions moveDirection)  
    {
        this._snake = snake;
        this._moveDirection = moveDirection;
    }

    public void Execute()
    {
        _snake.ChangeDirection(_moveDirection);
    }
}
