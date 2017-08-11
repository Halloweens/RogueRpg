using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GeneratorGrid))]
public class RoomGenerator : MonoBehaviour
{
    public GeneratorChunk prefab = null;

    public int numberOfRoom = 0;
    public int numberOfSecureRoom = 0;
    [SerializeField] private Vector2i roomMaxSize = Vector2i.zero;
    [SerializeField] private Vector2i roomMinSize = Vector2i.zero;

    private GeneratorGrid grid;

	private void Awake()
	{
		grid = GetComponent<GeneratorGrid>();
	}

	private Vector2i RandomSizeGenerator()
    {
        Vector2i roomSize = new Vector2i(Random.Range(roomMinSize.x, roomMaxSize.x + 1), Random.Range(roomMinSize.y, roomMaxSize.y + 1));

        return roomSize;
    }

    public IEnumerator CreateRooms()
    {
        int numberOfSecureRoomLeftToCreate = numberOfSecureRoom;
        for (int i = 0; i < numberOfRoom; ++i)
        {
            bool isSecure = false;
            if (numberOfSecureRoomLeftToCreate > 0)
            {
				isSecure = true;
				--numberOfSecureRoomLeftToCreate; 
            }
            
            Vector2i roomScale = RandomSizeGenerator();
            List<Vector2i> possibleRoomPos = GetPossibleRoomPos(roomScale);
            if (possibleRoomPos.Count <= 0)
                break;

            Vector2i roomPosition = possibleRoomPos[Random.Range(0, possibleRoomPos.Count)];

            grid.CreateRoom(roomPosition, prefab, roomScale, isSecure);

            yield return 0;
        }
    }

    private List<Vector2i> GetPossibleRoomPos(Vector2i roomScale)
    {
		if (!grid)
			grid = GetComponent<GeneratorGrid>();

        List<Vector2i> roomPossiblePosition = new List<Vector2i>();

        for (int j = 0; j < grid.areaSize.y; ++j)
        {
            for (int i = 0; i < grid.areaSize.x; ++i)
            {
                if (CanPlaceRoomAt(new Vector2i(i, j), roomScale))
                    roomPossiblePosition.Add(new Vector2i(i, j));
            }
        }

        return roomPossiblePosition;
    }

    private bool CanPlaceRoomAt(Vector2i position, Vector2i roomSize)
    {
        for (int j = 0; j < roomSize.y; j++)
        {
            for (int i = 0; i < roomSize.x; i++)
            {
                if (!grid.IsCoordValid(position.x + i, position.y + j) || grid.GetChunk(position.x + i, position.y + j) != null)
                    return false;
            }
        }

        return true;
    }
}
