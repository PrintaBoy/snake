using UnityEngine;
using System.Collections.Generic;
public class ConsumableController : MonoBehaviour
{
    [SerializeField] private List<Consumable> consumables;
    [SerializeField] private GameObject applePrefab;

    private void OnEnable()
    {
        SnakeController.OnSnakeSpawned += GenerateApple;
        Apple.OnAppleConsumed += GenerateApple;
    }

    private void OnDisable()
    {
        SnakeController.OnSnakeSpawned -= GenerateApple;
        Apple.OnAppleConsumed -= GenerateApple;
    }

    private void GenerateApple()
    {        
        GameObject generatedConsumable = Instantiate(applePrefab);
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
