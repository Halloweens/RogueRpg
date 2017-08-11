using UnityEngine;

abstract public class Item : ScriptableObject
{
	public enum ItemType : int
	{
		Equipment = 0,
		Weapon,
		Key,
		Consommable,
		Miscellaneous,
		QuestItem
	}

	public ItemType EnumItemType { get { return enumItemType; } protected set { enumItemType = value; } }
	protected ItemType enumItemType;

	public float Weight { get { return weight; } set { weight = value; } }
	[SerializeField] protected float weight;

	public string ItemName { get { return itemName; } set { itemName = value; } }
	[SerializeField] protected string itemName;

	public uint ItemValue { get { return itemValue; } set { itemValue = value; } }
	[SerializeField] protected uint itemValue;

	public Texture2D Icon { get { return icon; } set { icon = value; } }
	[SerializeField] protected Texture2D icon;

	public bool CanBeAddInInventory { get { return canBeAddInInventory; } set { canBeAddInInventory = value; } }
	[SerializeField] protected bool canBeAddInInventory = true;

}

public class ItemComparer : System.Collections.Generic.IEqualityComparer<Item>, System.Collections.Generic.IComparer<Item>
{
	public bool Equals(Item item1, Item item2)
	{
		if (item1.EnumItemType == item2.EnumItemType && item1.Weight == item2.Weight && item1.ItemName == item2.ItemName)
			return true;
		return false;
	}

	public int GetHashCode(Item item)
	{
		string code = item.EnumItemType.ToString() + "|" + item.Weight.ToString() + "|" + item.ItemName.ToString();
		return code.GetHashCode();
	}

	public int Compare(Item item1, Item item2)
	{
		if (item2.EnumItemType == item1.EnumItemType)
		{
			if (item1.EnumItemType == Item.ItemType.Equipment)
			{
				Equipment equipment1 = (Equipment)item1;
				Equipment equipment2 = (Equipment)item2;

				if (equipment1.Armor > equipment2.Armor)
					return -1;
				else if (equipment1.Armor < equipment2.Armor)
					return 1;
				else
					return 0;
			}
			else if (item1.EnumItemType == Item.ItemType.Weapon)
			{
				WeaponData weapon1 = (WeaponData)item1;
				WeaponData weapon2 = (WeaponData)item2;

				if (weapon1.Weapon.damages > weapon2.Weapon.damages)
					return -1;
				else if (weapon1.Weapon.damages < weapon2.Weapon.damages)
					return 1;
				else
					return 0;
			}
		}
		return 0;
	}
}
