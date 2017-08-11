using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public enum InterfaceType : int
	{
		PlayerInventory = 0,
		OtherInventory,
		Shop
	}

	public enum ShopMode : int
	{
		Buying = 0,
		Selling,
		None
	}

	public ButtonForInventory ButtonPrefab { get { return buttonPrefab; } private set { buttonPrefab = value; } }
	[SerializeField] private ButtonForInventory buttonPrefab;

	public Color ColorOfFavorite { get { return colorOfFavorite; } private set { colorOfFavorite = value; } }
	[SerializeField] private Color colorOfFavorite;

	[SerializeField] private FilterList filterList;
	public ItemList ItemList { get { return itemList; } private set { itemList = value; } }
	[SerializeField] private ItemList itemList;

	public InventoryLowerBar LowerBar { get { return lowerBar; } private set { lowerBar = value; } }
	[SerializeField] private InventoryLowerBar lowerBar;

	[SerializeField] private ItemCharacteristics itemCharaPanel;

	public HowMuchPanel HowMuchPanel { get { return howMuchPanel; } private set { howMuchPanel = value; } }
	[SerializeField] private HowMuchPanel howMuchPanel;

	public InterfaceType InventoryType { get { return inventoryType; } set { inventoryType = value; } }
	private InterfaceType inventoryType;

	public ShopMode ShopType { get { return shopMode; } private set { shopMode = value; } }
	private ShopMode shopMode =  ShopMode.None;

	public float MaxWeight { get { return maxWeight; } set { maxWeight = value; } }
	public float CurWeight { get { return curWeight; } set { curWeight = value; } }
	private float curWeight = 0f;
	private float maxWeight;

	public uint Gold { get { return gold; } set { gold = value; } }
	[SerializeField] private uint gold;

	public Dictionary<Item, uint> Items { get { return items; } set { items = value; } }
	private Dictionary<Item, uint> items = new Dictionary<Item, uint>(new ItemComparer());

	public Dictionary<Item, uint> FavoriteItems { get { return favoriteItems; } private set { favoriteItems = value; } }
	private Dictionary<Item, uint> favoriteItems = new Dictionary<Item, uint>(new ItemComparer());
	private bool favoriteDisplayed = false;

	public Item SelectedItem { get { return selectedItem; } set { selectedItem = value; } }
	private Item selectedItem = null;

	public Player Player { get { return player; } set { player = value; } }
	private Player player;

	[SerializeField] private List<Item> alreadyInInventory = new List<Item>();

	public bool encumbered { get { return curWeight >= maxWeight; } }
	public event System.Action onEncumberedChange;
	public event System.Action playerGainQuestItem;
	
	void Awake()
    {
		filterList.Inventory = this;
		itemList.Inventory = this;
		lowerBar.Inventory = this;
		howMuchPanel.Inventory = this;

		foreach (Item item in alreadyInInventory)
			AddItem(item);

		gameObject.SetActive(false);
	}

	public void DisplayInventory(Player _player)
	{
		player = _player;
		maxWeight = 50.0f + player.Characteristics.Strength * 2f;
		shopMode = ShopMode.Buying;
		if (inventoryType == InterfaceType.OtherInventory || inventoryType == InterfaceType.Shop)
			lowerBar.ShowTakeAllButton();
		gameObject.SetActive(true);

		gold = player.PlayerInventory.Gold;

		filterList.AddButtonFavorite();
		filterList.AddButtonAll().SelectButton();
		DisplayAll();

		List<Item.ItemType> typeAlreadyHere = new List<Item.ItemType>();

		foreach (Item item in items.Keys)
		{
			if (!typeAlreadyHere.Contains(item.EnumItemType))
			{
				filterList.AddButtonFilter(item);

				typeAlreadyHere.Add(item.EnumItemType);
			}
		}
		lowerBar.UpdateGoldLabel();
	}
	public void HideInventory()
	{
		itemList.ClearList();
		filterList.ClearList();
		selectedItem = null;
		howMuchPanel.Cancel();

		gameObject.SetActive(false);
	}

	public void SwitchDisplayedInventory(ShopMode mode)
	{
		itemList.ClearList();
		filterList.ClearList();
		selectedItem = null;

		DisplayOtherList();

		shopMode = mode;

		if (mode == ShopMode.Selling)
		{
			filterList.AddButtonFavorite();
			filterList.AddButtonAll().SelectButton();

			DisplayListOfItems(player.PlayerInventory.Items);

			List<Item.ItemType> typeAlreadyHere = new List<Item.ItemType>();

			foreach (Item item in player.PlayerInventory.Items.Keys)
			{
				if (!typeAlreadyHere.Contains(item.EnumItemType))
				{
					filterList.AddButtonFilter(item);

					typeAlreadyHere.Add(item.EnumItemType);
				}
			}
		}
		else
			DisplayInventory(player);
		lowerBar.UpdateGoldLabel();
	}

	public void DisplayAll()
	{
		DisplayOtherList();

		DisplayListOfItems(items);
		itemList.AddUpgradeArrow();
	}
	public void DisplayItems(Item.ItemType type)
	{
		DisplayOtherList();

		Dictionary<Item, uint> itemsToDisplay = GetItemsByType(type);

		DisplayListOfItems(itemsToDisplay);

		itemList.AddUpgradeArrow();
	}
	public void DisplayFavorite()
	{
		DisplayOtherList();
		favoriteDisplayed = true;

		DisplayListOfItems(favoriteItems);
		itemList.AddUpgradeArrow();
	}
	private void DisplayListOfItems(Dictionary<Item, uint> itemsToDisplay)
	{
		foreach (Item item in itemsToDisplay.Keys)
		{
			itemList.AddButtonItem(itemsToDisplay, item);
		}
	}

	private void DisplayOtherList()
	{
		howMuchPanel.Cancel();
		itemCharaPanel.Hide();
		selectedItem = null;
		itemList.ClearList();
		favoriteDisplayed = false;
		lowerBar.HideButtons();
		if (inventoryType == InterfaceType.OtherInventory || inventoryType == InterfaceType.Shop)
			lowerBar.ShowTakeAllButton();
	}

	public void DisplayItemCharacteristics(Item item, uint nb_item)
	{
		selectedItem = item;
		lowerBar.HideButtons();
		lowerBar.ShowButtons(item);
		itemCharaPanel.Display(item, nb_item);
		lowerBar.UpdateLowerBar();
		howMuchPanel.Cancel();
	}

	public Dictionary<Item, uint> GetItemsByType(Item.ItemType type)
	{
		Dictionary<Item, uint> itemsOfType = new Dictionary<Item, uint>(new ItemComparer());

		foreach (Item item in items.Keys)
			if (item.EnumItemType == type)
				itemsOfType.Add(item, items[item]);

		return itemsOfType;
	}

	public void AddItem(Item itemToAdd)
	{
		if (!itemToAdd.CanBeAddInInventory)
			return;

		if (itemToAdd is QuestItem && inventoryType == InterfaceType.PlayerInventory)
			if (playerGainQuestItem != null)
				playerGainQuestItem();

        if (items.ContainsKey(itemToAdd))
            ++items[itemToAdd];
        else
            items.Add(itemToAdd, 1);

		curWeight += itemToAdd.Weight;
		lowerBar.ChangeWeightLabel();
		if (inventoryType == InterfaceType.PlayerInventory && curWeight >= maxWeight)
		{
			player.GetComponent<CharController>().CanMove = false;
			if (onEncumberedChange != null)
				onEncumberedChange();
		}
	}
	public void RemoveItem(Item itemToRemove)
	{
		if (!items.ContainsKey(itemToRemove))
			return;

		if (items[itemToRemove] > 1)
		{
			--items[itemToRemove];
			ButtonForInventory button = itemList.FindButtonInItemListWithText(itemToRemove.ItemName);
			if (button)
				button.ChangeText(items[itemToRemove] > 1 ? itemToRemove.ItemName + " (" + items[itemToRemove] + ")"  : itemToRemove.ItemName);
		}
		else
		{
			if (player.Arsenal.HasItem(itemToRemove))
                player.Arsenal.Unequip(itemToRemove);
			items.Remove(itemToRemove);
			DeleteSelectedButton();
		}

		curWeight -= itemToRemove.Weight;
		if (inventoryType == InterfaceType.PlayerInventory && curWeight < maxWeight)
		{
			player.GetComponent<CharController>().CanMove = true;
			if (onEncumberedChange != null)
				onEncumberedChange();
		}

		lowerBar.ChangeWeightLabel();
	}
	public void DeleteSelectedButton()
	{
		if (!selectedItem)
			return;

		ButtonForInventory buttonToDestroy = itemList.FindButtonInItemListWithText(selectedItem.ItemName);
		if (buttonToDestroy)
		{
			if (buttonToDestroy.ArrowDisplayed())
				itemList.AddUpgradeArrow();
			Destroy(buttonToDestroy.gameObject);
		}

		selectedItem = null;
		itemCharaPanel.Hide();
		lowerBar.HideButtons();
		if (inventoryType == InterfaceType.OtherInventory || inventoryType == InterfaceType.Shop)
			lowerBar.ShowTakeAllButton();
		lowerBar.UpdateLowerBar();
	}

	public void ClickOnFavorite()
	{
		ButtonForInventory button = itemList.FindButtonInItemListWithText(selectedItem.ItemName);

		if (favoriteItems.ContainsKey(selectedItem))
		{
			favoriteItems.Remove(selectedItem);

			if (favoriteDisplayed)
			{
				Destroy(button.gameObject);

				selectedItem = null;
				itemCharaPanel.Hide();
				lowerBar.HideButtons();
			}
			else
			{
				lowerBar.ItemIsFavorite(false);
				button.InitTextColor();
			}

			return;
		}

		favoriteItems.Add(selectedItem, 1);
		button.ChangeTextColor(colorOfFavorite);
		lowerBar.ItemIsFavorite(true);
	}

	public void DestroyItem()
	{
		if (selectedItem is QuestItem)
			return;

		if (items[selectedItem] > 1)
		{
			howMuchPanel.Display();
			return;
		}

		RemoveItem(selectedItem);
	}

	public void RefreshWeight()
	{
		if (player)
			maxWeight = 50.0f + player.Characteristics.Strength * 2f;

		float newWeight = 0;
		foreach (Item item in items.Keys)
			newWeight += item.Weight * items[item];
		curWeight = newWeight;
		if (inventoryType == InterfaceType.PlayerInventory && curWeight >= maxWeight)
		{
			player.GetComponent<CharController>().CanMove = false;
			if (onEncumberedChange != null)
				onEncumberedChange();
		}
	}
}
