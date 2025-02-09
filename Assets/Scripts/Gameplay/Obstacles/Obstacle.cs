using UnityEngine;

public class Obstacle : MonoBehaviour, ISpawnable
{    
    public void SetupSpawnable(IGridTile parentTile)
    {
    }

    public void Collision()
    {
        Debug.Log("Collision!");
    }
}
