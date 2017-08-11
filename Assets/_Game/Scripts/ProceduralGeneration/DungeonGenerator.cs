using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(Pathfinder))]
[RequireComponent(typeof(GeneratorGrid))]
[RequireComponent(typeof(RoomGenerator))]
[RequireComponent(typeof(CorridorGenerator))]
[RequireComponent(typeof(WallGenerator))]
[RequireComponent(typeof(ChestGenerator))]
[RequireComponent(typeof(PlayerAndMerchantsGenerator))]
[RequireComponent(typeof(EnemiesGenerator))]


public class DungeonGenerator : MonoBehaviour {

	[SerializeField] private PortalToOtherLevel portal;
	[SerializeField] private Consommable teleportationToStartZoneScroll;
	[SerializeField] private SafeZone safeZoneCampfire;
    [SerializeField] private GameObject loadingScreen;
	[SerializeField] private Enemy boss;

    private RoomGenerator rooms;
    private CorridorGenerator corridors;
    private WallGenerator walls;
    private ChestGenerator chests;
	private PlayerAndMerchantsGenerator playerAndMerchantsGenerator;
	private EnemiesGenerator enemiesGenerator;

	public event System.Action portalCreated;
	public event System.Action bossCreated;

    void Start ()
    {
        rooms = GetComponent<RoomGenerator>();
        corridors = GetComponent<CorridorGenerator>();
        walls = GetComponent<WallGenerator>();
        chests = GetComponent<ChestGenerator>();
		playerAndMerchantsGenerator = GetComponent<PlayerAndMerchantsGenerator>();
		enemiesGenerator = GetComponent<EnemiesGenerator>();

        
        StartCoroutine(CreateDungeon());
        //
    }

    IEnumerator CreateDungeon()
    {
        GameObject loading = (GameObject)Instantiate(loadingScreen);
        yield return StartCoroutine(rooms.CreateRooms());
        yield return StartCoroutine(corridors.CreateCorridors());
        yield return StartCoroutine(walls.CreateRoomWalls());
        yield return StartCoroutine(walls.CreateCorridorWalls());
        yield return StartCoroutine(playerAndMerchantsGenerator.CreatePlayer());
        yield return StartCoroutine(playerAndMerchantsGenerator.CreateTraders());
		yield return StartCoroutine(chests.CreateChest());

		Pathfinder.Instance.CreateMap();

        SpawnPortal();
		CreateScroll();
		SpawnSafeZoneCampfire();
        yield return StartCoroutine(enemiesGenerator.CreateEnemies());
        Destroy(loading);
    }

	private void SpawnPortal()
	{
		GeneratorGrid grid = GetComponent<GeneratorGrid>();
		RoomList unSafeRoomList = grid.UnSafeRoomList;
		List<GeneratorChunk> room = unSafeRoomList[Random.Range(0, unSafeRoomList.Count)].chunks;

		GeneratorChunk chunk = room[Random.Range(0, room.Count)];

		float x = Random.Range(chunk.Location.x * (int)grid.chunkSize, (chunk.Location.x + 1) * (int)grid.chunkSize);
		float y = Random.Range(chunk.Location.y * (int)grid.chunkSize, (chunk.Location.y + 1) * (int)grid.chunkSize);

		PortalToOtherLevel newPortal = Instantiate(portal, new Vector3(x, 0, y), Quaternion.identity) as PortalToOtherLevel;
		newPortal.transform.SetParent(transform.parent);

		if (portalCreated != null)
			portalCreated();
	}

	public void SpawnBoss()
	{
		GeneratorGrid grid = GetComponent<GeneratorGrid>();
		RoomList unSafeRoomList = grid.UnSafeRoomList;
		List<GeneratorChunk> room = unSafeRoomList[Random.Range(0, unSafeRoomList.Count)].chunks;

		GeneratorChunk chunk = room[Random.Range(0, room.Count)];

		float x = Random.Range(chunk.Location.x * (int)grid.chunkSize, (chunk.Location.x + 1) * (int)grid.chunkSize);
		float y = Random.Range(chunk.Location.y * (int)grid.chunkSize, (chunk.Location.y + 1) * (int)grid.chunkSize);

		Enemy theBoss = Instantiate(boss, new Vector3(x, 0, y), Quaternion.identity) as Enemy;
		theBoss.transform.SetParent(transform.parent);

		if (bossCreated != null)
			bossCreated();
	}

	private void CreateScroll()
	{
		if (playerAndMerchantsGenerator.CreatedPlayer.PlayerInventory.Items.ContainsKey(teleportationToStartZoneScroll))
			return;

		if (chests.Chests.Count > 0)
		{
			InventoryToSearch chest = chests.Chests[Random.Range(0, chests.Chests.Count)].GetComponent<InventoryToSearch>();
			chest.AddItem(teleportationToStartZoneScroll);
		}
	}

	private void SpawnSafeZoneCampfire()
	{
		GeneratorGrid grid = GetComponent<GeneratorGrid>();
		List<GeneratorRoom> safeRoomList = grid.SafeRoomList;

		foreach (GeneratorRoom room in safeRoomList)
		{
			GeneratorChunk chunk = room.chunks[Random.Range(0, room.chunks.Count)];

			float x = Random.Range(chunk.Location.x * (int)grid.chunkSize, (chunk.Location.x + 1) * (int)grid.chunkSize);
			float y = Random.Range(chunk.Location.y * (int)grid.chunkSize, (chunk.Location.y + 1) * (int)grid.chunkSize);

			SafeZone newCampfireSafeZone = Instantiate(safeZoneCampfire, new Vector3(x, 0, y), Quaternion.identity) as SafeZone;
			newCampfireSafeZone.transform.SetParent(transform.parent);
		}
	}
}
