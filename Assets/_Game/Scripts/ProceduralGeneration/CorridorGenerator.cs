using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(GeneratorGrid))]
public class CorridorGenerator : MonoBehaviour
{

    public GeneratorChunk prefab = null;
    private GeneratorGrid grid;
    private RoomList roomList = new RoomList();

    void Awake()
    {
        grid = GetComponent<GeneratorGrid>();
    }

    public IEnumerator CreateCorridors()
    {
        foreach (GeneratorRoom room in grid.RoomList)
        {
            roomList.Add(room);
        }

        while (roomList.Count > 0)
        {
            Vector2i room1Pos = roomList[0].location;
            Vector2i room2Pos = FindNearestInList(room1Pos);
            if (room2Pos.x != Vector2i.zero.x || room2Pos.y != Vector2i.zero.y)
                grid.CreateCorridor(prefab, room1Pos, room2Pos);

            yield return 0;
        }
    }

    private Vector2i FindNearestInList(Vector2i pos)
    {
        roomList.Remove(roomList[0]);
        GeneratorRoom nearestRoom = null;
        float nearestDist = 100f;
        
        if (roomList.Count < 1)
            return grid.RoomList[0].location;
        
        else
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].location.DistTo(pos) < nearestDist)
                {
                    nearestDist = roomList[i].location.DistTo(pos);
                    nearestRoom = roomList[i];
                }
            }
        }
        if (nearestRoom != null)
            return nearestRoom.location;
        else
        {
            Debug.LogError("Possible bug with Walls");
            return Vector2i.zero;
        }
    }
}
