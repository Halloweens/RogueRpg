using UnityEngine;
using System.Collections;
using System.Collections.Generic;
    
[RequireComponent(typeof(GeneratorGrid))]
public class ChestGenerator : MonoBehaviour {

    [SerializeField] public TresureChest chest;

    private GeneratorGrid grid;
    private RoomList roomList;

    public int numberChest;

	public List<TresureChest> Chests { get { return chests; } }
	private List<TresureChest> chests = new List<TresureChest>();

	public IEnumerator CreateChest ()
	{
        grid = GetComponent<GeneratorGrid>();
        roomList = grid.RoomList;
        for (int i = 0; i < numberChest; i++)
        {
            int num = Random.Range(0, roomList.Count);
            List<GeneratorChunk> chunk = roomList[num].chunks;
            int choose = Random.Range(0, chunk.Count);
            float chunkPosX = chunk[choose].Location.x + 0.5f;
            float chunkPosY = chunk[choose].Location.y + 0.5f;
            int xPosition = Random.Range((int)chunkPosX * (int)grid.chunkSize, ((int)(chunkPosX  + 0.5f)) * (int)grid.chunkSize);
            int yPosition = Random.Range((int)chunkPosY * (int)grid.chunkSize, ((int)(chunkPosY + 0.5f)) * (int)grid.chunkSize);

            TresureChest newChest = Instantiate(chest, new Vector3(xPosition, 0f, yPosition), Quaternion.identity) as TresureChest;
			newChest.transform.SetParent(transform);
			chests.Add(newChest);

            yield return 0;
        }
    }

}
