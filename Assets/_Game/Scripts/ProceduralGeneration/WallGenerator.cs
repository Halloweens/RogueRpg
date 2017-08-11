using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GeneratorGrid))]
public class WallGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject wallOnSidePrefab = null;
    [SerializeField]
    private GameObject wallOnDownPrefab = null;
    [SerializeField]
    private GameObject wallWithDoorOnDownPrefab = null;
    [SerializeField]
    private GameObject wallWithDoorOnSidePrefab = null;

    private GeneratorGrid grid;

    void Awake()
    {
        grid = GetComponent<GeneratorGrid>();
    }

    public IEnumerator CreateRoomWalls()
    {
        foreach (GeneratorRoom room in grid.RoomList)
        {
            foreach (GeneratorChunk chunk in room.chunks)
            {
                Vector2i currPos = chunk.Location;

                Vector2i posUp = grid.GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Up);
                Vector2i posDown = grid.GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Down);
                Vector2i posOnRight = grid.GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Right);
                Vector2i posOnLeft = grid.GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Left);

                CreateWall(posUp, currPos, "up");
                CreateWall(posDown, currPos, "down");
                CreateWall(posOnLeft, currPos, "left");
                CreateWall(posOnRight, currPos, "right");

                yield return 0;
            }
        }
    }

    public IEnumerator CreateCorridorWalls()
    {
        foreach (GeneratorCorridor corridor in grid.CorridorList)
        {
            foreach (GeneratorChunk chunk in corridor.chunks)
            {
                Vector2i currPos = chunk.Location;

                Vector2i posUp = grid.GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Up);
                Vector2i posDown = grid.GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Down);
                Vector2i posOnRight = grid.GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Right);
                Vector2i posOnLeft = grid.GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Left);

                CreateWall(posUp, currPos, "up", false);
                CreateWall(posDown, currPos, "down", false);
                CreateWall(posOnLeft, currPos, "left", false);
                CreateWall(posOnRight, currPos, "right", false);

                yield return 0;
            }
        }
    }

    private void CreateWall(Vector2i pos, Vector2i currPos, string wallDir,bool isRoomWall = true)
    {
        if (!IsCorridorOrRoom(pos, isRoomWall))
        {
            GameObject wall;
            Vector2i position = new Vector2i(currPos.x * (int)grid.chunkSize, currPos.y * (int)grid.chunkSize);

            switch (wallDir)
            {
                case "up":
                    {
                        position += new Vector2i(1 * (int)grid.chunkSize, 1 * (int)grid.chunkSize);
                        if (grid.GetChunk(pos.x, pos.y) != null)
                            wall = grid.CreateWall(wallWithDoorOnDownPrefab, position, true);
                        else
                            wall = grid.CreateWall(wallOnDownPrefab, position, true);
                    };break;

                case "down":
                    {
                        if (grid.GetChunk(pos.x, pos.y) != null)
                            wall = grid.CreateWall(wallWithDoorOnDownPrefab, position);
                        else
                            wall = grid.CreateWall(wallOnDownPrefab, position);
                    }; break;

                case "left":
                    {
                        if (grid.GetChunk(pos.x, pos.y) != null)
                            wall = grid.CreateWall(wallWithDoorOnSidePrefab, position);
                        else
                            wall = grid.CreateWall(wallOnSidePrefab, position);
                    }; break;

                case "right":
                    {
                        position += new Vector2i(1 * (int)grid.chunkSize, 1 * (int)grid.chunkSize);
                        if (grid.GetChunk(pos.x, pos.y) != null)
                            wall = grid.CreateWall(wallWithDoorOnSidePrefab, position, true);
                        else
                            wall = grid.CreateWall(wallOnSidePrefab, position, true);
                    }; break;

                default: return;
            }

            wall.transform.SetParent(grid.GetChunk(currPos.x, currPos.y).transform.parent);
        }
    }

    private bool IsCorridorOrRoom(Vector2i position, bool isRoomWall)
    {
        if (grid.IsRoom(position.x, position.y) && isRoomWall)
            return true;

        if (grid.GetChunk(position.x, position.y) != null && !isRoomWall)
            return true;

        return false;
    }
}