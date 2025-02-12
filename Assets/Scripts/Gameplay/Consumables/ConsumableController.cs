using UnityEngine;
using System.Collections.Generic;
public class ConsumableController : MonoBehaviour
{
    [SerializeField] private List<Consumable> consumables;
    [SerializeField] private GameObject consumablePrefab;

    private void OnEnable()
    {
        SnakeController.OnSnakeSpawned += GenerateConsumable;
        Consumable.OnAppleConsumed += GenerateConsumable;
    }

    private void OnDisable()
    {
        SnakeController.OnSnakeSpawned -= GenerateConsumable;
        Consumable.OnAppleConsumed -= GenerateConsumable;
    }

    private void GenerateConsumable()
    {
        IGridTile emptyTile = GridController.instance.GetEmptyTile();
        GameObject generatedConsumable = Instantiate(consumablePrefab);

        if (generatedConsumable.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) // setup spawned consumable
        {
            spawnable.SetupSpawnable(emptyTile);
        }

        if (generatedConsumable.TryGetComponent<Consumable>(out Consumable consumable)) // add generated Consumable to list
        {
            consumables.Add(consumable);            
        }
    }
}
