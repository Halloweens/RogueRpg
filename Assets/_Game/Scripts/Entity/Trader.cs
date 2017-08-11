using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Usable))]
public class Trader : MonoBehaviour
{
	[SerializeField] private List<Item> itemsToSell = new List<Item>();

	public Dictionary<Item, uint> Items { get { return items; } set { items = value; } }
	private Dictionary<Item, uint> items = new Dictionary<Item, uint>(new ItemComparer());

	private UIRoot uiRoot = null;

	void Start ()
	{
		foreach (Item item in itemsToSell)
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
