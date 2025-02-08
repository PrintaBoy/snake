using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private Snake snakePrefab;


    private void OnEnable()
    {
        moveInput.action.started += MoveStarted;
    }

    private void OnDisable()
    {
        moveInput.action.started -= MoveStarted;
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
        }

        if (ctx.ReadValue<Vector2>() == down)
        {
            OnDownInput();
        }

        if (ctx.ReadValue<Vector2>() == right)
        {
            OnRightInput();
        }

        if (ctx.ReadValue<Vector2>() == left)
        {
            OnLeftInput();
        }
    }

    private void OnUpInput()
    {
        RunMoveCommand(snakePrefab, Directions.North);
    }

    private void OnDownInput()
    {
        RunMoveCommand(snakePrefab, Directions.South);
    }

    private void OnRightInput()
    {
        RunMoveCommand(snakePrefab, Directions.East);
    }

    private void OnLeftInput()
    {
        RunMoveCommand(snakePrefab, Directions.West);
    }

    private void RunMoveCommand(Snake snake, Directions moveDirection) // this method creates new move ICommand
    {
        if (snake == null)
        {
            return;
        }

        ICommand command = new MoveCommand(snake, moveDirection);
        CommandInvoker.ExecuteCommand(command); // this command needs to be executed ASAP so it's called from here
    }
}
