using UnityEngine;

public interface ISnakeSegment
{
    int snakeSegmentListIndex { get; }
    void SetListIndex(int listIndex);
    void MoveSnakeSegment(IGridTile tileToMoveTo);
    IGridTile GetParent();
    IGridTile GetPreviousParent();
    Directions GetLastMoveDirection();
    void DeleteSnakeSegment();
    Vector2Int GetParentGridAddress();
}
