using UnityEngine;

public interface IConsumable
{
    Vector2Int GetParentGridAddress();
    ConsumableTypes GetConsumableType();
}
