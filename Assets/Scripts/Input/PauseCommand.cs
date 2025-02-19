public class PauseCommand : ICommand
{
    private GameStateController _gameStateController;

    public PauseCommand(GameStateController gameStateController)
    {
        this._gameStateController = gameStateController;
    }

    public void Execute()
    {
        _gameStateController.ChangePauseState();
    }
}
