using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Usable))]
public class InventoryToSearch : MonoBehaviour
{
	[SerializeField] private List<Item> possessedItems = new List<Item>();

	public Dictionary<Item, uint> Items { get { return items; } set { items = value; } }
	private Dictionary<Item, uint> items = new Dictionary<Item, uint>(new ItemComparer());

	private UIRoot uiRoot = null;

	void Start()
	{
		foreach (Item item in possessedItems)
			AddItem(item);

		GetComponent<Usable>().onUsable.AddListener(OnUseableCallback);

		uiRoot = GameObject.FindGameObjectWithTag("UIRoot").GetComponent<UIRoot>();
	}

	public void AddItem(Item itemToAdd)
	{
		if (items.ContainsKey(itemToAdd))
			++items[itemToAdd];
		else
			items.Add(itemToAdd, 1);
	}

	void OnUseableCallback(OnUsableArg arg)
	{
		if (!uiRoot.UIRootNonPlayer.Displayed)
			uiRoot.UIRootNonPlayer.DisplayInventory(gameObject);
	}
}
