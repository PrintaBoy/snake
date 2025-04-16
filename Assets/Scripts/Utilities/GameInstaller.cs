using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private GameStateController gameStateController;

    public override void InstallBindings()
    {
        Container.Bind<SnakeController>().FromInstance(snakeController).AsSingle();
        Container.Bind<GameStateController>().FromInstance(gameStateController).AsSingle();
    }
}
