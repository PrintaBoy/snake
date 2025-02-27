using UnityEngine;
using System;

public class Consumable : MonoBehaviour
{
    [HideInInspector] public IGridTile parent;
    [SerializeField] private GameObject consumeParticle;
    [HideInInspector] public int scoreValue;

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

    public virtual void DespawnConsumable()
    {
        parent = null;
        gameObject.SetActive(false);
        Instantiate(consumeParticle, gameObject.transform.position, gameObject.transform.rotation); // spawn consume particle
    }
}
