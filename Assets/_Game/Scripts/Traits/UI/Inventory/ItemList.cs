using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
	[SerializeField] private Transform listContent;

	public Inventory Inventory { get { return inventory; } set { inventory = value; } }
	private Inventory inventory;

	public void AddButtonItem(Dictionary<Item, uint> items, Item item)
	{
		ButtonForInventory button = Instantiate(inventory.ButtonPrefab);
		button.transform.SetParent(listContent);
		button.ChangeText(items[item] > 1 ? item.ItemName + " (" + items[item] + ")" : item.ItemName);

		if (inventory.FavoriteItems.ContainsKey(item))
			button.ChangeTextColor(inventory.ColorOfFavorite);

		button.button.onClick.AddListener(delegate { inventory.DisplayItemCharacteristics(item, items[item]); });

		if (inventory.Player.Arsenal.HasItem(item))
			button.DisplayImageEquipped();
	}

	public ButtonForInventory FindButtonInItemListWithText(string text)
	{
		for (int idx = 0; idx < listContent.childCount; ++idx)
		{
			string buttonText = listContent.GetChild(idx).GetComponentInChildren<Text>().text;

			if (buttonText.IndexOfAny("(".ToCharArray()) != -1)
				buttonText = buttonText.Substring(0, buttonText.IndexOfAny("(".ToCharArray()) - 1);

			if (buttonText == text)
				return listContent.GetChild(idx).GetComponent<ButtonForInventory>();
		}

		return null;
	}

	public void AddUpgradeArrow()
	{
		List<Item> weapons = new List<Item>();
		List<Item> shields = new List<Item>();
		List<Item> helmets = new List<Item>();
		List<Item> chests = new List<Item>();
		List<Item> boots = new List<Item>();

		foreach (Item item in inventory.Items.Keys)
		{
			if (item.EnumItemType == Item.ItemType.Weapon)
				weapons.Add(item);
			else if (item.EnumItemType == Item.ItemType.Equipment)
			{
				Equipment equipment = (Equipment)item;
				if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Head)
					helmets.Add(item);
				else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Shield)
					shields.Add(item);
				else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Chest)
					chests.Add(item);
				else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Feet)
					boots.Add(item);
			}
		}

		SortAndAddUpgradeArrow(weapons);
		SortAndAddUpgradeArrow(shields);
		SortAndAddUpgradeArrow(helmets);
		SortAndAddUpgradeArrow(chests);
		SortAndAddUpgradeArrow(boots);

	}

	private void SortAndAddUpgradeArrow(List<Item> items)
	{
		if (items.Count > 0)
		{
			items.Sort(new ItemComparer());
			ButtonForInventory button = FindButtonInItemListWithText(items[0].ItemName);
			if (button)
				button.DisplayUpgradeArrow();
		}
	}

	public void ClearList()
	{
		for (int idx = listContent.childCount - 1; idx >= 0; --idx)
			Destroy(listContent.GetChild(idx).gameObject);
	}
}
