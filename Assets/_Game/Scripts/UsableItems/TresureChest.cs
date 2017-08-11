using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InventoryToSearch))]
public class TresureChest : MonoBehaviour
{
	public ItemsList AllItems { get { return allItems; } set { allItems = value; } }
	[SerializeField] private ItemsList allItems;

	[SerializeField] private uint nbMaxItemsToCarry;

	void Start()
	{
		InventoryToSearch inv = GetComponent<InventoryToSearch>();
		int nbObj = (int)Random.Range(0, nbMaxItemsToCarry);
		for (int idx = 0; idx < nbObj; ++idx)
			inv.AddItem(randomItem());
	}

	protected Item randomItem()
	{
		return allItems.AllItems[Random.Range(0, allItems.AllItems.Count)];
	}
}
