using UnityEngine;
using System.Collections.Generic;
public class ConsumableController : MonoBehaviour
{
    public static ConsumableController instance;

    [HideInInspector] public List<Consumable> consumables;
    [SerializeField] private ObjectPool appleObjectPool;
    [SerializeField] private ObjectPool pumpkinObjectPool;
    [SerializeField] private ObjectPool mushroomObjectPool;
    [SerializeField] private ObjectPool acornObjectPool;
    [SerializeField] private ObjectPool grapeObjectPool;

    // keeps track of how many ticks have passed to spawn consumables
    private int appleTickCounter = 0;
    private int pumpkinTickCounter = 0; 
    private int mushroomTickCounter = 0;
    private int acornTickCounter = 0;
    private int grapeTickCounter = 0;

    private void Awake()
    {
        instance = this;
    }

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
        Grape.OnGrapeConsumed += GrapeConsumed;
        Grape.OnGrapeDespawn += GrapeConsumed;
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
        Grape.OnGrapeConsumed += GrapeConsumed;
        Grape.OnGrapeDespawn += GrapeConsumed;
        TickController.OnGameTick -= GameTick;
    }

    private void GameTick()
    {
        appleTickCounter++;
        pumpkinTickCounter++;
        mushroomTickCounter++;
        acornTickCounter++;
        grapeTickCounter++;        
        CheckConsumableSpawnConditions();
    }

    private void CheckConsumableSpawnConditions()
    {
        if (appleTickCounter >= GameData.gameData.appleSpawnRate && CanSpawnConsumable(ConsumableTypes.Apple))
        {
            GenerateConsumable(appleObjectPool, GridController.instance.GetEmptyTile());
            appleTickCounter = 0;
        }

        if (pumpkinTickCounter >= GameData.gameData.pumpkinSpawnRate)
        {
            GenerateConsumable(pumpkinObjectPool, GridController.instance.GetEmptyTile());
            pumpkinTickCounter = 0;
        }

        if (mushroomTickCounter >= GameData.gameData.mushroomSpawnRate)
        {
            GenerateConsumable(mushroomObjectPool, GridController.instance.GetEmptyTile());
            mushroomTickCounter = 0;
        }

        if (acornTickCounter >= GameData.gameData.acornSpawnRate)
        {
            GenerateConsumable(acornObjectPool, GridController.instance.GetEmptyTile());
            acornTickCounter = 0;   
        }

        if (grapeTickCounter >= GameData.gameData.grapeSpawnRate)
        {
            GenerateConsumable(grapeObjectPool, GridController.instance.GetEmptyTile());
            grapeTickCounter = 0;
        }
    }

    private void AppleConsumed(Apple apple)
    {
        consumables.Remove(apple);        
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

    private void GrapeConsumed(Grape grape)
    {
        consumables.Remove(grape);
    }

    private void SnakeSpawned()
    {
        if (!SceneController.isNewGame)
        {
            for (int i = 0; i < GameData.gameData.consumableListAmount; i++)
            {
                switch (GameData.gameData.consumableListTypes[i])
                {
                    case ConsumableTypes.Acorn:
                        GenerateConsumable(acornObjectPool, GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                    case ConsumableTypes.Apple:
                        GenerateConsumable(appleObjectPool, GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                    case ConsumableTypes.Grape:
                        GenerateConsumable(grapeObjectPool, GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                    case ConsumableTypes.Mushroom:
                        GenerateConsumable(mushroomObjectPool, GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                    case ConsumableTypes.Pumpkin:
                        GenerateConsumable(pumpkinObjectPool, GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                }
            }
            // spawn saved consumables
        } else
        {
            GenerateConsumable(appleObjectPool, GridController.instance.GetEmptyTile()); // here to spawn apple right at the beginning of the level
        }        
    }

    private void GenerateConsumable(ObjectPool objectPool, IGridTile spawnTile)
    {
        GameObject generatedConsumable = objectPool.GetPooledObject();
        generatedConsumable.SetActive(true);
        SetupConsumable(generatedConsumable, spawnTile);
    }

    private void SetupConsumable(GameObject spawnedConsumable, IGridTile generatedConsumableTile)
    {
        if (spawnedConsumable.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) // setup spawned consumable
        {
            spawnable.SetupSpawnable(generatedConsumableTile);
        }

        if (spawnedConsumable.TryGetComponent<Consumable>(out Consumable consumable)) // add generated Consumable to list
        {
            consumables.Add(consumable);
        }
    }

    private bool CanSpawnConsumable(ConsumableTypes consumableType)
    {
        int consumableCount = 0;
        foreach (Consumable consumable in consumables)
        {
            if (consumable.GetConsumableType() == consumableType)
            {
                consumableCount++;
            }
        }

        return consumableCount < GameData.gameData.appleMaxAmount;
    }
}
