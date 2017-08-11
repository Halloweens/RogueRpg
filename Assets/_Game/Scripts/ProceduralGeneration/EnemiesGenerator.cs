using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(GeneratorGrid))]
public class EnemiesGenerator : MonoBehaviour
{
	[SerializeField] private Enemy enemy;
	[SerializeField] private ItemsList allItems;

	private GeneratorGrid grid;
	private RoomList unSafeRoomList;

	[SerializeField] private uint numberOfEnemies;

	public event Action onAllEnemiesCreated;

	void Awake()
	{
		grid = GetComponent<GeneratorGrid>();
		unSafeRoomList = grid.UnSafeRoomList;
	}
	
	public IEnumerator CreateEnemies()
	{
		for (int idx = 0; idx < numberOfEnemies; ++idx)
		{
			List<GeneratorChunk> room = unSafeRoomList[UnityEngine.Random.Range(0, unSafeRoomList.Count)].chunks;

			GeneratorChunk chunk = room[UnityEngine.Random.Range(0, room.Count)];
			float x = UnityEngine.Random.Range(chunk.Location.x * (int)grid.chunkSize, (chunk.Location.x + 1) * (int)grid.chunkSize);
			float y = UnityEngine.Random.Range(chunk.Location.y * (int)grid.chunkSize, (chunk.Location.y + 1) * (int)grid.chunkSize);

			Enemy newEnemy = Instantiate(enemy, new Vector3(x, 1.5f, y), Quaternion.identity) as Enemy;
			newEnemy.transform.SetParent(transform.parent);
			newEnemy.AllItems = allItems;

            yield return 0;
		}

		FireAllEnemiesCreated();
	}

	private void FireAllEnemiesCreated()
	{
		if (onAllEnemiesCreated != null)
			onAllEnemiesCreated();
	}
}
