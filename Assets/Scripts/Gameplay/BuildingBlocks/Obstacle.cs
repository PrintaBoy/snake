using UnityEngine;

public class Obstacle : MonoBehaviour, IObstacle
{
    public void GenerateGridBorders()
    {
        GameData gDataRef = GameData.gameData; // here to make code little cleaner

        for (int i = -1; i < GameData.gameData.levelWidth + 1; i++) // make bottom row
        {
            Instantiate(gameObject, new Vector3((gDataRef.generateLevelStartPoint.x) + (gDataRef.gridSize * i), 0, gDataRef.generateLevelStartPoint.z - gDataRef.gridSize), gameObject.transform.rotation);
        }

        for (int i = -1; i < GameData.gameData.levelWidth + 1; i++) // make top row
        {
            Instantiate(gameObject, new Vector3((gDataRef.generateLevelStartPoint.x) + (gDataRef.gridSize * i), 0, -gDataRef.generateLevelStartPoint.z + gDataRef.gridSize), gameObject.transform.rotation);
        }        

        for (int i = 0; i < GameData.gameData.levelHeight; i++) // make left column
        {
            Instantiate(gameObject, new Vector3(gDataRef.generateLevelStartPoint.x - gDataRef.gridSize, 0, (gDataRef.generateLevelStartPoint.z) + (gDataRef.gridSize * i)), gameObject.transform.rotation);
        }

        for (int i = 0; i < GameData.gameData.levelHeight; i++) // make right column
        {
            Instantiate(gameObject, new Vector3(-gDataRef.generateLevelStartPoint.x + gDataRef.gridSize, 0, (gDataRef.generateLevelStartPoint.z) + (gDataRef.gridSize * i)), gameObject.transform.rotation);
        }
    }

    public void GenerateObstacle()
    {

    }
}
