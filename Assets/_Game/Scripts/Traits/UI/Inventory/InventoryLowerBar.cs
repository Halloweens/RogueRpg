using UnityEngine;
using UnityEngine.UI;

public class InventoryLowerBar : MonoBehaviour
{
	[SerializeField] private Text damagesLabel;
	[SerializeField] private Text modifierLabel;
	[SerializeField] private Text weightLabel;
	[SerializeField] private Text goldLabel;

	[SerializeField] private Button destroyButton;
	[SerializeField] private Button favoriteButton;
	[SerializeField] private Button equipButton;

	[SerializeField] private Button takeButton;
	[SerializeField] private Button takeAllButton;

	[System.NonSerialized] public string weightText;
	private string goldText;

	public Inventory Inventory { get { return inventory; } set { inventory = value; } }
	private Inventory inventory;

	void Awake()
	{
		goldText = goldLabel.text;
		inventory = transform.parent.GetComponent<Inventory>();

		ChangeWeightLabel();
	}

	public void UpdateLowerBar()
	{
		Item item = inventory.SelectedItem;

		if (!item)
		{
			damagesLabel.text = "";
			modifierLabel.text = "";
			return;
		}

		Player player = inventory.Player;

		if (inventory.InventoryType == Inventory.InterfaceType.Shop)
		{
			if (inventory.ShopType == Inventory.ShopMode.Buying)
				takeButton.GetComponentInChildren<Text>().color = inventory.Player.PlayerInventory.Gold >= item.ItemValue ? Color.green : Color.red;
			else
				takeButton.GetComponentInChildren<Text>().color = Color.white;
		}

		if (item.EnumItemType == Item.ItemType.Weapon)
		{
			ChangeLowerBarStat(item, player.Arsenal.RightHandWeapon);
		}

		else if (item.EnumItemType == Item.ItemType.Equipment)
		{
			Equipment equipment = (Equipment)item;
			if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Head)
				ChangeLowerBarStat(equipment, player.Arsenal.HeadArmor);
			else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Chest)
				ChangeLowerBarStat(equipment, player.Arsenal.ChestArmor);
			else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Feet)
				ChangeLowerBarStat(equipment, player.Arsenal.FeetsArmor);
			else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Shield)
				ChangeLowerBarStat(equipment, player.Arsenal.Shield);
		}

		else
		{
			damagesLabel.text = "";
			modifierLabel.text = "";
		}
	}
	private void ChangeLowerBarStat(Item item, Item playerItem)
	{
		string armorOrDamages = item.EnumItemType == Item.ItemType.Weapon ? "Damages" : "Armor";
		uint itemStat = item.EnumItemType == Item.ItemType.Weapon ? ((WeaponData)item).Weapon.damages : ((Equipment)item).Armor;

		if (playerItem)
		{
			uint playerStat = playerItem.EnumItemType == Item.ItemType.Weapon ? ((WeaponData)playerItem).Weapon.damages : ((Equipment)playerItem).Armor;
			ChangeParticularStat(armorOrDamages, playerStat);
			ChangeModifierLabel((int)(itemStat - playerStat));
		}
		else
		{
			ChangeParticularStat(armorOrDamages, 0);
			ChangeModifierLabel((int)itemStat);
		}
	}

	public void ChangeParticularStat(string armorOrDamages, uint value)
	{
		damagesLabel.text = armorOrDamages + " : " + value;
	}

	public void ChangeModifierLabel(int modifier)
	{
		if (modifier == 0)
		{
			modifierLabel.text = "";
			return;
		}
		modifierLabel.color = modifier > 0 ? Color.green : Color.red;

		modifierLabel.text = "( " + modifier + " )";
	}

	public void ChangeWeightLabel()
	{
		if (weightText == null)
			weightText = weightLabel.text;

		float curWeight = inventory.CurWeight;
		if (inventory.InventoryType == Inventory.InterfaceType.OtherInventory || inventory.InventoryType == Inventory.InterfaceType.Shop)
			foreach (Item item in inventory.Items.Keys)
				curWeight += item.Weight;

		weightLabel.text = string.Format(weightText, curWeight, inventory.MaxWeight);
	}

	public void UpdateGoldLabel()
	{
		if (goldText == null)
			goldText = goldLabel.text;

		goldLabel.text = string.Format(goldText, inventory.Player.PlayerInventory.Gold);
	}

	public void ShowTakeAllButton()
	{
		if (!inventory)
			inventory = transform.parent.GetComponent<Inventory>();

		takeAllButton.gameObject.SetActive(true);
		takeAllButton.GetComponentInChildren<Text>().text = inventory.InventoryType == Inventory.InterfaceType.Shop ? "Switch" : "Take All";
	}
	public void ShowButtons(Item item)
	{
		if (inventory.InventoryType == Inventory.InterfaceType.PlayerInventory)
			PlayerButtons(item);
		else if (inventory.InventoryType == Inventory.InterfaceType.OtherInventory)
		{
			OtherInventoryButtons();
		}
		else if (inventory.InventoryType == Inventory.InterfaceType.Shop)
		{
			ShopButtons();
		}
	}

	private void PlayerButtons(Item item)
	{
		destroyButton.gameObject.SetActive(true);
		favoriteButton.gameObject.SetActive(true);

		ItemIsFavorite(inventory.FavoriteItems.ContainsKey(item));

		if (item.EnumItemType == Item.ItemType.Weapon || item.EnumItemType == Item.ItemType.Equipment)
		{
			equipButton.gameObject.SetActive(true);
			ItemIsEquipped(inventory.Player.Arsenal.HasItem(item));
		}
		else if (item.EnumItemType == Item.ItemType.Consommable)
		{
			equipButton.GetComponentInChildren<Text>().text = "Use";
			equipButton.gameObject.SetActive(true);
		}
	}

	private void OtherInventoryButtons()
	{
		takeButton.gameObject.SetActive(true);
		ShowTakeAllButton();
	}

	private void ShopButtons()
	{
		takeButton.gameObject.SetActive(true);
		takeButton.GetComponentInChildren<Text>().text = inventory.ShopType == Inventory.ShopMode.Buying ? "Buy" : "Sell";
		ShowTakeAllButton();
	}

	public void TakeItem()
	{
		if (!inventory.HowMuchPanel.Displayed)
		{
			if ((inventory.InventoryType == Inventory.InterfaceType.PlayerInventory || (inventory.InventoryType == Inventory.InterfaceType.Shop && inventory.ShopType == Inventory.ShopMode.Selling)) && inventory.Player.PlayerInventory.Items[inventory.SelectedItem] > 1)
			{
				inventory.HowMuchPanel.Display();
				return;
			}
			else if (inventory.InventoryType == Inventory.InterfaceType.Shop && inventory.ShopType == Inventory.ShopMode.Buying && inventory.Items[inventory.SelectedItem] > 1)
			{
				inventory.HowMuchPanel.Display();
				return;
			}
		}

		if (inventory.InventoryType == Inventory.InterfaceType.Shop)
		{
			if (inventory.ShopType == Inventory.ShopMode.Buying)
			{
				if (inventory.Player.PlayerInventory.Gold >= inventory.SelectedItem.ItemValue)
				{
					inventory.Player.PlayerInventory.Gold -= inventory.SelectedItem.ItemValue;
					UpdateGoldLabel();
				}
				else
					return;
			}
			else if (inventory.ShopType == Inventory.ShopMode.Selling)
			{
				inventory.Player.PlayerInventory.Gold += inventory.SelectedItem.ItemValue;
				UpdateGoldLabel();
				inventory.AddItem(inventory.SelectedItem);
				inventory.Player.PlayerInventory.RemoveItem(inventory.SelectedItem);
				if (inventory.Player.Arsenal.HasItem(inventory.SelectedItem))
					inventory.Player.Arsenal.Unequip(inventory.SelectedItem);
				if (!inventory.Player.PlayerInventory.Items.ContainsKey(inventory.SelectedItem))
					inventory.DeleteSelectedButton();

				return;
			}
		}
		inventory.Player.PlayerInventory.AddItem(inventory.SelectedItem);
		inventory.RemoveItem(inventory.SelectedItem);
	}

	public void TakeAll()
	{
		if (inventory.InventoryType == Inventory.InterfaceType.Shop)
		{
			inventory.SwitchDisplayedInventory(inventory.ShopType == Inventory.ShopMode.Buying ? Inventory.ShopMode.Selling : Inventory.ShopMode.Buying);
			return;
		}

		foreach (Item item in inventory.Items.Keys)
		{
			for (int idx = 0; idx < inventory.Items[item]; ++idx)
				inventory.Player.PlayerInventory.AddItem(item);
		}

		inventory.Items.Clear();
		inventory.CurWeight = 0;
		inventory.ItemList.ClearList();
	}

	public void ClickOnEquip()
	{
		Item selectedItem = inventory.SelectedItem;

		if (selectedItem.EnumItemType == Item.ItemType.Weapon || selectedItem.EnumItemType == Item.ItemType.Equipment)
		{
			ButtonForInventory button = inventory.ItemList.FindButtonInItemListWithText(inventory.SelectedItem.ItemName);

			if (inventory.Player.Arsenal.HasItem(selectedItem))
			{
				Unequip(selectedItem, button);
				return;
			}
			Equip(selectedItem, button);
			return;
		}
		Consumme(selectedItem);
	}

	private void Unequip(Item item, ButtonForInventory button)
	{
		inventory.Player.Arsenal.Unequip(item);
		button.HideImageEquipped();
		ItemIsEquipped(false);
		ChangeModifierLabel(item is Equipment ? (int)((Equipment)item).Armor : (int)((WeaponData)item).Weapon.damages);
		ChangeParticularStat(item is Equipment ? "Armor" : "Damages", 0);
	}
	private void Equip(Item item, ButtonForInventory button)
	{
		Item prevEquipped = inventory.Player.Arsenal.Equip(item);
		if (prevEquipped)
		{
			ButtonForInventory prevButton = inventory.ItemList.FindButtonInItemListWithText(prevEquipped.ItemName);
			if (prevButton)
				prevButton.HideImageEquipped();
		}
		button.DisplayImageEquipped();
		ItemIsEquipped(true);
		ChangeModifierLabel(0);
		ChangeParticularStat(item is Equipment ? "Armor" : "Damages", item is Equipment ? ((Equipment)item).Armor : ((WeaponData)item).Weapon.damages);
	}
	private void Consumme(Item item)
	{
		Consommable consommable = (Consommable)item;

		if (consommable.EnumAffectation == Consommable.Affectation.Health)
		{
			inventory.Player.Damageable.Hp = consommable.BonusValue + inventory.Player.Damageable.hp >= inventory.Player.Damageable.maxHP ? inventory.Player.Damageable.maxHP : consommable.BonusValue + inventory.Player.Damageable.hp;
		}
		else if (consommable.EnumAffectation == Consommable.Affectation.Mana)
		{
			inventory.Player.ManaMgr.GainMana(consommable.BonusValue);
		}
		else if (consommable.EnumAffectation == Consommable.Affectation.Teleportation)
		{
			SaveData();
			TimeManager.Instance.Pause = false;
			inventory.RemoveItem(item);
			UnityEngine.SceneManagement.SceneManager.LoadScene(consommable.TeleportationSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
			UnityEngine.SceneManagement.SceneManager.UnloadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
			return;
		}
		inventory.RemoveItem(item);
	}

	public void ItemIsFavorite(bool favorite)
	{
		favoriteButton.GetComponentInChildren<Text>().text = favorite ? "Unfavorite" : "Favorite";
	}

	public void ItemIsEquipped(bool equipped)
	{
		equipButton.GetComponentInChildren<Text>().text = equipped ? "Unequip" : "Equip";
	}

	public void HideButtons()
	{
		destroyButton.gameObject.SetActive(false);
		favoriteButton.gameObject.SetActive(false);
		equipButton.gameObject.SetActive(false);
		takeButton.gameObject.SetActive(false);
		takeAllButton.gameObject.SetActive(false);
	}

	private void SaveData()
	{
		BetweenLevelsData bld = UnityEngine.SceneManagement.SceneManager.GetSceneByName("BetweenLevelsScene").GetRootGameObjects()[0].GetComponent<BetweenLevelsData>();

		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		bld.playerInventoryItems = player.PlayerInventory.Items;
		bld.playerGold = player.PlayerInventory.Gold;
		bld.playerName = player.GetComponent<Entity>().entityName;
		bld.hp = player.GetComponent<Damageable>().Hp;
		bld.mana = player.GetComponent<ManaManager>().Mana;
		bld.xp = player.Characteristics.Experience;
		bld.playerCharacteristics.remaining = player.Characteristics.RemainingPoints;
		bld.playerCharacteristics.targetXP = player.Characteristics.TargetExp;
		bld.level = player.Characteristics.Level;

		bld.playerCharacteristics.strength = player.Characteristics.Strength;
		bld.playerCharacteristics.constitution = player.Characteristics.Constitution;
		bld.playerCharacteristics.intelligence = player.Characteristics.Intelligence;
		bld.playerCharacteristics.dexterity = player.Characteristics.Dexterity;

		bld.playerArsenal.headArmor = player.Arsenal.HeadArmor;
		bld.playerArsenal.chestArmor = player.Arsenal.ChestArmor;
		bld.playerArsenal.feetsArmor = player.Arsenal.FeetsArmor;
		bld.playerArsenal.rightHandWeapon = player.Arsenal.RightHandWeapon;
		bld.playerArsenal.leftHandWeapon = player.Arsenal.LeftHandWeapon;
		bld.playerArsenal.shield = player.Arsenal.Shield;

		bld.spells = player.Arsenal.spells;

		bld.timeSinceFirstStart = TimeManager.Instance.TimeSinceFirstStart;
	}
}
