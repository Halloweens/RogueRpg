using UnityEngine;
using System;

public class UIRootNonPlayer : MonoBehaviour
{
	[SerializeField] private MenuPause menuPause;
	[SerializeField] private Inventory inventory;

	public Player Player { get { return player; } set { player = value; } }
	private Player player;

	private GameObject curSource = null;

	public bool Displayed { get { return displayed; } private set { displayed = value; } }
	private bool displayed = false;

	public event Action inventoryDisplayChange;

	void Start ()
	{
	
	}

	void LateUpdate()
	{
		if (displayed && Input.GetKeyDown(KeyCode.Escape))
			HideInventory();
	}
	
	public void DisplayInventory(GameObject source)
	{
		if (source.GetComponent<Trader>())
		{
			curSource = source;
			inventory.Items = source.GetComponent<Trader>().Items;
			inventory.InventoryType = Inventory.InterfaceType.Shop;
			inventory.DisplayInventory(player);
			displayed = true;
			TimeManager.Instance.Pause = true;
			OnInventoryDisplayChange();
		}
		else if (source.GetComponent<InventoryToSearch>())
		{
			curSource = source;
			inventory.Items = source.GetComponent<InventoryToSearch>().Items;
			inventory.InventoryType = Inventory.InterfaceType.OtherInventory;
			inventory.DisplayInventory(player);
			displayed = true;
			TimeManager.Instance.Pause = true;
			OnInventoryDisplayChange();
		}
	}

	public void HideInventory()
	{
		if (curSource.GetComponent<Trader>())
			curSource.GetComponent<Trader>().Items = inventory.Items;
		else if (curSource.GetComponent<InventoryToSearch>())
			curSource.GetComponent<InventoryToSearch>().Items = inventory.Items;

		curSource = null;
		inventory.HideInventory();
		displayed = false;
		TimeManager.Instance.Pause = false;
		OnInventoryDisplayChange();
	}

	private void OnInventoryDisplayChange()
	{
		if (inventoryDisplayChange != null)
			inventoryDisplayChange();
	}
}
