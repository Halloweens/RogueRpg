using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RoomList : List<GeneratorRoom> { }
[System.Serializable]
public class CorridorList : List<GeneratorCorridor> { }

[RequireComponent(typeof(Pathfinder))]

public class GeneratorGrid : MonoBehaviour
{
    [SerializeField] public float chunkSize;
    [SerializeField] public Vector2i areaSize;

    public GameObject parent;
    private GeneratorChunk[,] chunks = new GeneratorChunk[0, 0];

	public RoomList RoomList { get { return roomList; } }
	private RoomList roomList = new RoomList();

	public RoomList SafeRoomList { get { return safeRoomList; } }
	private RoomList safeRoomList = new RoomList();

	public RoomList UnSafeRoomList { get { return unSafeRoomList; } }
	private RoomList unSafeRoomList = new RoomList();

	public CorridorList CorridorList { get { return corridorList; } }
	private CorridorList corridorList = new CorridorList();

    public GeneratorChunk[,] Chunks { get { return chunks; } set { chunks = value; } }

    Pathfinder pathfinder;

    private void Awake()
    {
        chunks = new GeneratorChunk[areaSize.x, areaSize.y];
        pathfinder = GetComponent<Pathfinder>();
        pathfinder.MapStartPosition = Vector2.zero;
        pathfinder.MapEndPosition = new Vector2(areaSize.x * chunkSize + 10, areaSize.y * chunkSize + 10);
    }

    public bool IsRoom(int xPosition, int yPosition)
    {
        GeneratorChunk chunk = GetChunk(xPosition, yPosition);
        if (chunk == null)
            return false;

        foreach (GeneratorRoom room in roomList)
        {
            if (room.chunks.Find(x => x == chunk) != null)
                return true;
        }

        return false;
    }

    public bool IsCoordValid(int xCoord, int yCoord)
    {
        if (xCoord < 0 || xCoord >= areaSize.x)
            return false;
        if (yCoord < 0 || yCoord >= areaSize.y)
            return false;

        return true;
    }

    public GeneratorChunk GetChunk(int xPosition, int yPosition)
    {
        if (!IsCoordValid(xPosition, yPosition))
            return null;

		return chunks[xPosition, yPosition];
    }

    private GeneratorChunk CreateChunk(Vector2i chunkPos, GeneratorChunk prefab, Color color, GameObject parent = null)
    {
        if (chunks[chunkPos.x, chunkPos.y] != null)
            return null;

        GeneratorChunk chunk = Instantiate(prefab) as GeneratorChunk;
        //GeneratorChunk ceiling = (GeneratorChunk)Instantiate(prefab);
        chunk.Initialize(this, chunkPos);
        //ceiling.Initialize(this, chunkPos);

        chunks[chunkPos.x, chunkPos.y] = chunk;

        GameObject chunkGo = chunk.gameObject;
        //GameObject ceilingGo = ceiling.gameObject;

        chunkGo.name = "Ground";
        //ceilingGo.name = "Ceiling";

        chunkGo.transform.position = new Vector3(chunkPos.x * chunkSize + chunkGo.transform.position.x, 0f, chunkPos.y * chunkSize + chunkGo.transform.position.z);
        //ceilingGo.transform.position = new Vector3(chunkPos.x * chunkSize + ceilingGo.transform.position.x, 5f, chunkPos.y * chunkSize + ceilingGo.transform.position.z);
        //to modify
        chunkGo.transform.localScale = new Vector3(chunkSize / 10, 1f, chunkSize / 10);
        //ceilingGo.transform.localScale = new Vector3(chunkSize / 10, 1f, chunkSize / 10);

        //Debug//chunkGo.GetComponentInChildren<Renderer>().material.color = color;
        //ceilingGo.GetComponentInChildren<Renderer>().material.color = color;

        if (parent != null)
        {
            chunkGo.transform.SetParent(parent.transform);
            //ceilingGo.transform.SetParent(parent.transform);
        }


        return chunk;
    }

    public GeneratorRoom CreateRoom(Vector2i chunkPos, GeneratorChunk groundPrefab, Vector2i roomSize, bool isSecure = false)
    {
        GeneratorRoom newRoom = new GeneratorRoom();
        GameObject room = new GameObject();
        room.transform.SetParent(parent.transform);
        Color testColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);

        room.name = "Room";

        for (int i = 0; i < roomSize.y; i++)
        {
            for (int j = 0; j < roomSize.x; j++)
            {
                GeneratorChunk c = CreateChunk(chunkPos + new Vector2i(j, i), groundPrefab, testColor, room);
                newRoom.chunks.Add(c);
            }
        }

		if (isSecure)
		{
			newRoom.isSafe = true;
			room.name = "SecureRoom";
			safeRoomList.Add(newRoom);
		}
		else
			unSafeRoomList.Add(newRoom);

        newRoom.location = chunkPos;
        newRoom.size = roomSize;

        roomList.Add(newRoom);

        return newRoom;
    }

    public GeneratorCorridor CreateCorridor(GeneratorChunk groundPrefab, Vector2i room1Pos, Vector2i room2Pos)
    {
        GeneratorCorridor newCorridor = new GeneratorCorridor();

        Color testColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        Vector2i currPos = room1Pos;
        GameObject corridor = new GameObject();
        corridor.transform.SetParent(parent.transform);
        corridor.name = "Corridor";

        while (currPos.x != room2Pos.x || currPos.y != room2Pos.y)
        {
            while (currPos.y != room2Pos.y)
            {
                if (!IsRoom(currPos.x, currPos.y) && IsCoordValid(currPos.x, currPos.y))
                {
                    GeneratorChunk c = CreateChunk(currPos, groundPrefab, testColor, corridor);
                    if (c != null)
                    {
                        newCorridor.chunks.Add(c);
                    }
                }
                // Bug appen
                if (GetChunk(currPos.x, currPos.y) == null)
                {
                    Debug.LogError("Bug possible with Corridor");
                    return null;
                }

                if (room2Pos.y > currPos.y)
                    currPos = GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Up);
                else if (room2Pos.y < currPos.y)
                    currPos = GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Down);
                else
                    break;
                //
            }

            if (!IsRoom(currPos.x, currPos.y) && IsCoordValid(currPos.x, currPos.y))
            {
                GeneratorChunk c = CreateChunk(currPos, groundPrefab, testColor, corridor);
                if (c != null)
                {
                    newCorridor.chunks.Add(c);
                }
            }
            //Bug appen
            if (GetChunk(currPos.x, currPos.y) == null)
            {
                Debug.LogError("Bug possible with Corridor");
                return null;
            }

            if (room2Pos.x > currPos.x)
                currPos = GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Right);
            else if (room2Pos.x < currPos.x)
                currPos = GetChunk(currPos.x, currPos.y).Location + GeneratorDirections.ToVector2i(GeneratorDirection.Left);
            else
                break;
            //
        }

        if (corridor.transform.childCount < 1)
            Destroy(corridor);

        corridorList.Add(newCorridor);

        return newCorridor;
    }

    public GameObject CreateWall(GameObject wallPrefab, Vector2i wallPos, bool IsRotate = false)
    {
        Vector3 position = new Vector3(wallPos.x, 0f, wallPos.y);
        GameObject wall = (GameObject)Instantiate(wallPrefab, position, Quaternion.identity);
        if (IsRotate)
            wall.transform.Rotate(Vector3.up, 180f);

        return wall;
    }
}
