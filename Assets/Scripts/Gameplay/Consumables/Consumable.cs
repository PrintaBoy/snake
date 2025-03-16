using UnityEngine;
using System;

public class Consumable : MonoBehaviour
{
    [HideInInspector] public IGridTile parent;
    [SerializeField] private GameObject consumeParticle;
    [SerializeField] private ConsumableTypes consumableType;
    [HideInInspector] public int scoreValue;

    public virtual void OnEnable()
    {
        SnakeController.OnSnakeCollision += Collision;
    }

    public virtual void OnDisable()
    {
        SnakeController.OnSnakeCollision -= Collision;
    }

    public virtual void Collision(ISpawnable collisionObject)
    {
    }

    public virtual Vector2Int GetParentGridAddress()
    {
        return parent.GetGridTileAddress();
    }

    public ConsumableTypes GetConsumableType()
    {
        return consumableType;
    }

    public virtual void DespawnConsumable()
    {
        parent = null;
        gameObject.SetActive(false);
        Instantiate(consumeParticle, gameObject.transform.position, gameObject.transform.rotation); // spawn consume particle
    }
}
