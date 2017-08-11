using UnityEngine;
using UnityEngine.UI;

public class HowMuchPanel : MonoBehaviour
{
	[SerializeField] private Text howMuchLabel;
	[SerializeField] private Button buttonPlus;
	[SerializeField] private Button buttonLess;
	[SerializeField] private Text numberLabel;

	public Inventory Inventory { get { return inventory; } set { inventory = value; } }
	private Inventory inventory;

	private string howMuchText;
	private uint number = 0;

	public bool Displayed { get { return displayed; } }
	private bool displayed = false;

	void Awake()
	{
		howMuchText = howMuchLabel.text;
	}

	public void Display()
	{
		if (!gameObject.activeInHierarchy)
		{
			displayed = true;
			if (howMuchText == null)
				howMuchText = howMuchLabel.text;

			if (inventory.InventoryType == Inventory.InterfaceType.PlayerInventory)
				howMuchLabel.text = string.Format(howMuchText, "destroy");
			else if (inventory.InventoryType == Inventory.InterfaceType.OtherInventory)
				howMuchLabel.text = string.Format(howMuchText, "take");
			else if (inventory.InventoryType == Inventory.InterfaceType.Shop)
				howMuchLabel.text = inventory.ShopType == Inventory.ShopMode.Buying ? "buy" : "sell";

			numberLabel.text = number.ToString();

			gameObject.SetActive(true);
		}
	}

	public void Add()
	{
		uint nbItems = 0;
		if (inventory.InventoryType == Inventory.InterfaceType.Shop)
			nbItems = inventory.ShopType == Inventory.ShopMode.Buying ? inventory.Items[inventory.SelectedItem] : inventory.Player.PlayerInventory.Items[inventory.SelectedItem];
		else
			nbItems = inventory.Items[inventory.SelectedItem];
		if (number + 1 <= nbItems)
		{
			++number;
			numberLabel.text = number.ToString();
		}
	}

	public void Remove()
	{
		if (number > 0)
		{
			--number;
			numberLabel.text = number.ToString();
		}
	}

	public void Validate()
	{
		Item selectedItem = inventory.SelectedItem;

		for (uint idx = 0; idx < number; ++idx)
		{
			if (inventory.InventoryType == Inventory.InterfaceType.PlayerInventory)
				inventory.RemoveItem(selectedItem);
			else
				inventory.LowerBar.TakeItem();
			inventory.SelectedItem = selectedItem;
		}

		number = 0;
		gameObject.SetActive(false);
		displayed = false;
	}

	public void Cancel()
	{
		number = 0;
		gameObject.SetActive(false);
		displayed = false;
	}
}
