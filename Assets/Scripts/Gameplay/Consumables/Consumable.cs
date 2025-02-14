using UnityEngine;
using System;

public class Consumable : MonoBehaviour
{
    [HideInInspector] public IGridTile parent;

    public void OnEnable()
    {
        SnakeController.OnSnakeCollision += Collision;
    }

    public void OnDisable()
    {
        SnakeController.OnSnakeCollision -= Collision;
    }

    public virtual void Collision(ISpawnable collisionObject)
    {
    }
}
