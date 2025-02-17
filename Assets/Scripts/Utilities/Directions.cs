public enum Directions
{
    North,
    South,
    East,
    West
}

public class Direction
{
    public static Directions GetOppositeDirection(Directions originalDirection) // return opposite direction
    {
        Directions oppositeDirection = Directions.East;
        switch (originalDirection) 
        {
            case Directions.North:
                oppositeDirection = Directions.South;
                break;
            case Directions.South:
                oppositeDirection = Directions.North;
                break;
            case Directions.East:
                oppositeDirection = Directions.West;
                break;
            case Directions.West:
                oppositeDirection = Directions.East;
                break;
        }
        return oppositeDirection;
    }
}



