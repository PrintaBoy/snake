using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference pauseMenuInput;
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private GameStateController gameStateController;

    private void OnEnable()
    {
        moveInput.action.started += MoveStarted;
        pauseMenuInput.action.started += ChangePauseStarted;
        ButtonEventInvoker.OnResumeButtonPressed += RunChangePauseCommand;
    }

    private void OnDisable()
    {
        moveInput.action.started -= MoveStarted;
        pauseMenuInput.action.started -= ChangePauseStarted;
        ButtonEventInvoker.OnResumeButtonPressed -= RunChangePauseCommand;
    }

    private void ChangePauseStarted(InputAction.CallbackContext ctx)
    {
        RunChangePauseCommand();
    }

    private void MoveStarted(InputAction.CallbackContext ctx) // here read move input from player and pass move directions
    {   
        Vector2 up = new Vector2(0.00f, 1.00f);
        Vector2 down = new Vector2(0.00f, -1.00f);
        Vector2 right = new Vector2(1.00f, 0.00f);
        Vector2 left = new Vector2(-1.00f, 0.00f);        

        if (ctx.ReadValue<Vector2>() == up)
        {
            OnUpInput();
            return;
        }

        if (ctx.ReadValue<Vector2>() == down)
        {
            OnDownInput();
            return;
        }

        if (ctx.ReadValue<Vector2>() == right)
        {
            OnRightInput();
            return;
        }

        if (ctx.ReadValue<Vector2>() == left)
        {
            OnLeftInput();
            return;
        }
    }
    private void OnUpInput()
    {
        RunMoveCommand(snakeController, Directions.North);
    }

    private void OnDownInput()
    {
        RunMoveCommand(snakeController, Directions.South);
    }

    private void OnRightInput()
    {
        RunMoveCommand(snakeController, Directions.East);
    }

    private void OnLeftInput()
    {
        RunMoveCommand(snakeController, Directions.West);
    }

    private void RunMoveCommand(SnakeController snake, Directions moveDirection) // this method creates new move ICommand
    {
        if (snake == null)
        {
            return;
        }

        ICommand command = new MoveCommand(snake, moveDirection);
        CommandInvoker.ExecuteCommand(command); // this command needs to be executed ASAP so it's called from here
    }

    private void RunChangePauseCommand()
    {
        ICommand command = new PauseCommand(gameStateController);
        CommandInvoker.ExecuteCommand(command);
    }
}
