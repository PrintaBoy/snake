using UnityEngine;
using System.Collections.Generic;
public class ConsumableController : MonoBehaviour
{
    [SerializeField] private List<Consumable> consumables;
    [SerializeField] private GameObject consumablePrefab;

    private void GenerateConsumable()
    {
        // upon receiving an event from game manager, here will be generated spawnable object in a grid
        // will check for empty grid tile in GridDictionary        
    }
}
