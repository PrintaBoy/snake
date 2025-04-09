using System.Collections.Generic;
using UnityEngine;

public interface IGridController
{
    void AddToGridDictionary(Vector2Int address, IGridTile gridTile);
    void AdjecentTilesMapGenerated();
    IGridTile GetTileOnAddress(Vector2Int address);
    IGridTile GetRandomEmptyTile();
    IGridTile GetEmptyTileOutsideSafeZone(int safeZoneRadius);
}
