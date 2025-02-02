using UnityEngine;

public class LevelGenerator : MonoBehaviour
{    
    [SerializeField] private GameObject[] generatableBlocks;
    
    private void Start()
    {
        GameData.gameData.CalculateGenerateStartPoint();
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        foreach (GameObject generatableBlock in generatableBlocks)
        {
            if (generatableBlock.TryGetComponent<IGeneratable>(out IGeneratable generatable))
            {
                generatable.Generate();
            } else
            {
                Debug.LogError(generatable + "does not have IGeneratable interface");
            }
        }
    }
}
