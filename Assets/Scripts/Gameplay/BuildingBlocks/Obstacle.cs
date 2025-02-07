using UnityEngine;

public class Obstacle : MonoBehaviour, ISpawnable
{    
    public void Spawn()
    {
    }

    public void Collision()
    {
        Debug.Log("Collision!");
    }
}
