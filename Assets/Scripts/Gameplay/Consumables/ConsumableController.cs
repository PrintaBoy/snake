using UnityEngine;
using System.Collections.Generic;
class ConsumableController : MonoBehaviour
{
    /// <summary>
    /// This class spawns and keeps track of consumables    
    /// </summary>
    
    public static ConsumableController instance;

    public List<IConsumable> consumables {  get; private set; }
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
        consumables = new List<IConsumable>();
        instance = this;
    }

    private void OnEnable()
    {
        SnakeController.OnSnakeSpawned += SnakeSpawned;   
        Consumable.OnConsumableConsumed += ConsumableConsumed;        
        TickController.OnGameTick += GameTick;
    }

    private void OnDisable()
    {
        SnakeController.OnSnakeSpawned -= SnakeSpawned;
        Consumable.OnConsumableConsumed -= ConsumableConsumed;
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
            SetupConsumable(GenerateConsumable(appleObjectPool), GridController.instance.GetEmptyTile());
            appleTickCounter = 0;
        }

        if (pumpkinTickCounter >= GameData.gameData.pumpkinSpawnRate)
        {         
            SetupConsumable(GenerateConsumable(pumpkinObjectPool), GridController.instance.GetEmptyTile());
            pumpkinTickCounter = 0;
        }

        if (mushroomTickCounter >= GameData.gameData.mushroomSpawnRate)
        {         
            SetupConsumable(GenerateConsumable(mushroomObjectPool), GridController.instance.GetEmptyTile());
            mushroomTickCounter = 0;
        }

        if (acornTickCounter >= GameData.gameData.acornSpawnRate)
        {         
            SetupConsumable(GenerateConsumable(acornObjectPool), GridController.instance.GetEmptyTile());
            acornTickCounter = 0;   
        }

        if (grapeTickCounter >= GameData.gameData.grapeSpawnRate)
        {            
            SetupConsumable(GenerateConsumable(grapeObjectPool), GridController.instance.GetEmptyTile());
            grapeTickCounter = 0;
        }
    }

    private void ConsumableConsumed(ConsumableTypes consumableType)
    {
        foreach (IConsumable consumableInList in consumables)
        {
            if (consumableInList.GetConsumableType() == consumableType)
            {
                consumables.Remove(consumableInList);
                return;
            }
        }
    }

    private void SnakeSpawned() // either loads consumables from save or starts as new game
    {
        if (!SceneController.isNewGame)
        {
            for (int i = 0; i < GameData.gameData.consumableListAmount; i++)
            {
                switch (GameData.gameData.consumableListTypes[i])
                {
                    case ConsumableTypes.Acorn:
                        SetupConsumable(GenerateConsumable(acornObjectPool), GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                    case ConsumableTypes.Apple:                        
                        SetupConsumable(GenerateConsumable(appleObjectPool), GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                    case ConsumableTypes.Grape:                        
                        SetupConsumable(GenerateConsumable(grapeObjectPool), GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                    case ConsumableTypes.Mushroom:                        
                        SetupConsumable(GenerateConsumable(mushroomObjectPool), GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                    case ConsumableTypes.Pumpkin:                        
                        SetupConsumable(GenerateConsumable(pumpkinObjectPool), GridController.instance.gridDictionary[GameData.gameData.consumableListAddresses[i]]);
                        break;
                }
            }            
        } else
        {
            SetupConsumable(GenerateConsumable(appleObjectPool), GridController.instance.GetEmptyTile()); // here to spawn apple right at the beginning of the level
        }        
    }

    private GameObject GenerateConsumable(ObjectPool objectPool)
    {
        GameObject generatedConsumable = objectPool.GetPooledObject();
        generatedConsumable.SetActive(true);
        return generatedConsumable;        
    }

    private void SetupConsumable(GameObject spawnedConsumable, IGridTile generatedConsumableTile)
    {
        if (spawnedConsumable.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) // setup spawned consumable
        {
            spawnable.SetupSpawnable(generatedConsumableTile);            
        }

        if (spawnedConsumable.TryGetComponent<IConsumable>(out IConsumable consumable)) // add generated Consumable to list
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

        int spawnMaximum = 0;
        switch (consumableType)
        {
            case ConsumableTypes.Apple:
                spawnMaximum = GameData.gameData.appleMaxAmount;
                break;
            case ConsumableTypes.Acorn:
                break;
            case ConsumableTypes.Mushroom:
                break;
            case ConsumableTypes.Pumpkin:
                break;
            case ConsumableTypes.Grape:
                break;
        }

        return consumableCount < spawnMaximum;
    }
}
