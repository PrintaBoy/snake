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
        GameObject generatedConsumable = Instantiate(consumablePrefab, emptyTile.gameObject.transform.position, emptyTile.gameObject.transform.rotation);

        if (generatedConsumable.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) //setup spawned snake segment
        {
            spawnable.SetupSpawnable(emptyTile);
        }

        if (generatedConsumable.TryGetComponent<Consumable>(out Consumable consumable)) // add generated SnakeSegment to list
        {
            consumables.Add(consumable);            
        }
    }
}
