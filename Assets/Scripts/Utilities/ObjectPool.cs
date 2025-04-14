using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// This class is used to pool objects so there is no need for instancing. This allows to reuse assets in scene and helps with performance
    /// To use, simply attach it to game object and reference game objects you wish to pool into List of gameobjects in inspector
    /// </summary>

    [SerializeField] private List<GameObject> pooledObjects;

    public GameObject GetPooledObject()
    {
        /// <summary>
        /// Returns active GameObject from pooledObjects list     
        /// </summary>

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
}
