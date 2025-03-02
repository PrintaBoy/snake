using UnityEngine;
using System.Collections.Generic;
public class ConsumableController : MonoBehaviour
{
    [SerializeField] private List<Consumable> consumables;    
    [SerializeField] private ObjectPool appleObjectPool;
    [SerializeField] private ObjectPool pumpkinObjectPool;
    [SerializeField] private ObjectPool mushroomObjectPool;
    [SerializeField] private ObjectPool acornObjectPool;

    // keeps track of how many ticks have passed to spawn consumables
    private int pumpkinTickCounter = 0; 
    private int mushroomTickCounter = 0;
    private int acornTickCounter = 0;

    private void OnEnable()
    {
        SnakeController.OnSnakeSpawned += SnakeSpawned;        
        Apple.OnAppleConsumed += AppleConsumed;
        Pumpkin.OnPumpkinConsumed += PumpkinConsumed;
        Pumpkin.OnPumpkinDespawn += PumpkinConsumed;
        Mushroom.OnMushroomConsumed += MushrooomConsumed;
        Mushroom.OnMushroomDespawn += MushrooomConsumed;
        Acorn.OnAcornDespawn += AcornConsumed;
        Acorn.OnAcornConsumed += AcornConsumed;
        TickController.OnGameTick += GameTick;
    }

    private void OnDisable()
    {
        SnakeController.OnSnakeSpawned -= SnakeSpawned;        
        Apple.OnAppleConsumed -= AppleConsumed;
        Pumpkin.OnPumpkinConsumed -= PumpkinConsumed;
        Pumpkin.OnPumpkinDespawn -= PumpkinConsumed;
        Mushroom.OnMushroomConsumed -= MushrooomConsumed;
        Mushroom.OnMushroomDespawn -= MushrooomConsumed;
        Acorn.OnAcornDespawn -= AcornConsumed;
        Acorn.OnAcornConsumed -= AcornConsumed;
        TickController.OnGameTick -= GameTick;
    }

    private void GameTick()
    {
        pumpkinTickCounter++;
        mushroomTickCounter++;
        acornTickCounter++;
        CheckConsumableSpawnConditions();
    }

    private void CheckConsumableSpawnConditions()
    {
        if (pumpkinTickCounter >= GameData.gameData.pumpkinSpawnRate)
        {
            GenerateConsumable(pumpkinObjectPool);
            pumpkinTickCounter = 0;
        }

        if (mushroomTickCounter >= GameData.gameData.mushroomSpawnRate)
        {
            GenerateConsumable(mushroomObjectPool);
            mushroomTickCounter = 0;
        }

        if (acornTickCounter >= GameData.gameData.acornSpawnRate)
        {
            GenerateConsumable(acornObjectPool);
            acornTickCounter = 0;   
        }
    }

    private void AppleConsumed(Apple apple)
    {
        consumables.Remove(apple);
        GenerateConsumable(appleObjectPool);
    }

    private void PumpkinConsumed(Pumpkin pumpkin)
    {
        consumables.Remove(pumpkin);        
    }

    private void MushrooomConsumed(Mushroom mushroom)
    {
        consumables.Remove(mushroom);        
    }

    private void AcornConsumed(Acorn acorn)
    {
        consumables.Remove(acorn);
    }

    private void SnakeSpawned()
    {
        GenerateConsumable(appleObjectPool);        
    }

    private void GenerateConsumable(ObjectPool objectPool)
    {
        GameObject generatedConsumable = objectPool.GetPooledObject();
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
