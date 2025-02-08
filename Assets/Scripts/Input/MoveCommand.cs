public class MoveCommand : ICommand
{
    private Snake _snake;
    private Directions _moveDirection;

    public MoveCommand(Snake snake, Directions moveDirection)
    {
        this._snake = snake;
        this._moveDirection = moveDirection;
    }

    public void Execute()
    {
        _snake.ChangeDirection(_moveDirection);
    }
}
