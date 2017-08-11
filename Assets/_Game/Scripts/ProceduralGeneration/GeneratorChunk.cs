using UnityEngine;
using System.Collections;

public class GeneratorChunk : MonoBehaviour
{
    public GeneratorWall[] walls;

    public bool IsRoom { get { return grid.IsRoom(location.x, location.y); } }

    private GeneratorGrid grid = null;
    public Vector2i Location { get { return location; } }
    private Vector2i location = Vector2i.zero;

    public void Initialize(GeneratorGrid grid, Vector2i location)
    {
        this.grid = grid;
        this.location = location;
    }

    public GeneratorChunk GetNeighbor(GeneratorDirection dir)
    {
        Vector2i loc = location + GeneratorDirections.ToVector2i(dir);
        return grid.GetChunk(loc.x, loc.y);
    }
}

public enum GeneratorDirection : int
{
    Up = 0,
    Down,
    Left,
    Right
}

public class GeneratorDirections
{
    public static Vector2i ToVector2i(GeneratorDirection dir)
    {
        switch (dir)
        {
            case GeneratorDirection.Up:
                return new Vector2i(0, 1);
            case GeneratorDirection.Down:
                return new Vector2i(0, -1);
            case GeneratorDirection.Left:
                return new Vector2i(-1, 0);
            case GeneratorDirection.Right:
                return new Vector2i(1, 0);
            default:
                throw new System.Exception("Invalid switch value.");
        }
    }
}
