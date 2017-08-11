using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(GeneratorGrid))]
public class PlayerAndMerchantsGenerator : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private Trader trader;
	[SerializeField] private UIRoot uiRoot;
	[SerializeField] private GameCamera gameCamera;

	private GeneratorGrid grid;
	private RoomList safeRoomList;

	public uint numberTrader;

	List<GeneratorChunk> safeRoom = null;

	public Player CreatedPlayer { get { return createdPlayer; } }
	private Player createdPlayer;

	void Awake ()
	{
		grid = GetComponent<GeneratorGrid>();
		safeRoomList = grid.SafeRoomList;
	}
	
	public IEnumerator CreatePlayer()
	{
		safeRoom = safeRoomList[Random.Range(0, safeRoomList.Count)].chunks;

		GeneratorChunk chunk = safeRoom[Random.Range(0, safeRoom.Count)];
        float chunkPosX = chunk.Location.x + 0.5f;
        float chunkPosY = chunk.Location.y + 0.5f;
        int x = Random.Range((int)chunkPosX * (int)grid.chunkSize, ((int)(chunkPosX + 0.5f)) * (int)grid.chunkSize);
        int y = Random.Range((int)chunkPosY * (int)grid.chunkSize, ((int)(chunkPosY + 0.5f)) * (int)grid.chunkSize);
        

		Player newPlayer = Instantiate(player, new Vector3(x, 0.1f, y), Quaternion.identity) as Player;
		newPlayer.transform.SetParent(transform.parent);
		createdPlayer = newPlayer;

		CreateCamera(newPlayer);
		CreateUIRoot(newPlayer);

        yield return 0;
	}

	private void CreateUIRoot(Player newPlayer)
	{
		UIRoot newUIRoot = Instantiate(uiRoot) as UIRoot;
		newUIRoot.transform.SetParent(transform.parent);
	}
	
	private void CreateCamera(Player newPlayer)
	{
		GameCamera newGameCamera = Instantiate(gameCamera) as GameCamera;
		newGameCamera.modelRenderer = newPlayer.transform.GetChild(0).GetComponent<Renderer>();
		newGameCamera.center = newPlayer.CameraFocus;
		Camera.SetupCurrent(newGameCamera.GetComponent<Camera>());
		newGameCamera.transform.SetParent(transform.parent);
		newPlayer.GetComponent<PlayerInputSystem>().playerCamera = newGameCamera.GetComponent<Camera>();
	}

	public IEnumerator CreateTraders()
	{
		for (int idx = 0; idx < numberTrader; ++idx)
		{
			GeneratorChunk chunk = safeRoom[Random.Range(0, safeRoom.Count)];
            float chunkPosX = chunk.Location.x + 0.5f;
            float chunkPosY = chunk.Location.y + 0.5f;
            int x = Random.Range((int)chunkPosX * (int)grid.chunkSize, ((int)(chunkPosX + 0.5f)) * (int)grid.chunkSize);
            int y = Random.Range((int)chunkPosY * (int)grid.chunkSize, ((int)(chunkPosY + 0.5f)) * (int)grid.chunkSize);

            Trader newTrader = Instantiate(trader, new Vector3(x, 0, y), Quaternion.identity) as Trader;
			newTrader.transform.SetParent(transform.parent);

            yield return 0;
		}
	}
}
