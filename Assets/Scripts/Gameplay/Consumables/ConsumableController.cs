using UnityEngine;
using System.Collections.Generic;
public class ConsumableController : MonoBehaviour
{
    [SerializeField] private List<Consumable> consumables;    
    [SerializeField] private ObjectPool appleObjectPool;
    [SerializeField] private ObjectPool pumpkinObjectPool;

    private void OnEnable()
    {
        SnakeController.OnSnakeSpawned += GenerateApple;
        SnakeController.OnSnakeSpawned += GeneratePumpkin;
        Apple.OnAppleConsumed += AppleConsumed;
        Pumpkin.OnPumpkinConsumed += PumpkinConsumed;
    }

    private void OnDisable()
    {
        SnakeController.OnSnakeSpawned -= GenerateApple;
        SnakeController.OnSnakeSpawned -= GeneratePumpkin;
        Apple.OnAppleConsumed -= AppleConsumed;
    }

    private void AppleConsumed(Apple apple)
    {
        consumables.Remove(apple);
        GenerateApple();
    }

    private void PumpkinConsumed(Pumpkin pumpkin)
    {
        consumables.Remove(pumpkin);
        GeneratePumpkin();
    }

    private void GeneratePumpkin()
    {
        GameObject generatedConsumable = pumpkinObjectPool.GetPooledObject();
        generatedConsumable.SetActive(true);
        SetupConsumable(generatedConsumable);
    }

    private void GenerateApple()
    {
        GameObject generatedConsumable = appleObjectPool.GetPooledObject();
        generatedConsumable.SetActive(true);
        SetupConsumable(generatedConsumable);
    }

    private void SetupConsumable(GameObject spawnedConsumable)
    {
        if (spawnedConsumable.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) // setup spawned consumable
        {
            spawnable.SetupSpawnable(GridController.instance.GetEmptyTile());
        }

        if (spawnedConsumable.TryGetComponent<Consumable>(out Consumable consumable)) // add generated Consumable to list
        {
            consumables.Add(consumable);
        }
    }
}
