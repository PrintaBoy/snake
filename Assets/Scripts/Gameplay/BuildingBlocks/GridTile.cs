using UnityEngine;

public class GridTile : MonoBehaviour, IGridTile
{    
    private Grid grid;
    private Vector2Int gridAddress;
    private ISpawnable spawnableObject;

    private void Awake()
    {        
        grid = GameObject.Find("LevelManager").GetComponent<Grid>();
        Debug.Log(gridAddress);        
        spawnableObject = null;
    }

    public void Generate()
    {
        GameData gDataRef = GameData.gameData; // here to make code little cleaner
        for (int i = 0; i < GameData.gameData.levelWidth; i++)
        {
            for (int j = 0; j < GameData.gameData.levelHeight; j++)
            {                
                GameObject generatedTile = Instantiate(gameObject, new Vector3((gDataRef.generateLevelStartPoint.x) + (gDataRef.gridSize * i), 0, (gDataRef.generateLevelStartPoint.z) + (gDataRef.gridSize * j)), gameObject.transform.rotation);                
                generatedTile.GetComponent<GridTile>().SetupGridTile(new Vector2Int(i, j));                
            }
        }
    }

    public void SetupGridTile(Vector2Int gridTileAddress)
    {
        gridAddress = gridTileAddress;
        name = "GridTile_" + gridTileAddress.x.ToString() + "_" + gridTileAddress.y.ToString();
        grid.AddToGridDictionary(gridAddress, this);
    }

    public bool HasObject()
    {
        bool hasObject = spawnableObject == null ? false : true;
        return hasObject;
    }
}