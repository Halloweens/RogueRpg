using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equipment")]
public class Equipment : Item
{
	public uint Armor { get { return armor; } set { armor = value; } }
	[SerializeField] private uint armor = 0;

	public enum EquipmentEmplacement : int
	{
		Head = 0,
		Chest,
		Feet,
		Shield
	}

	public EquipmentEmplacement EnumEquipmentEmplacement { get { return enumEquipmentEmplacement; } set { enumEquipmentEmplacement = value; } }
	[SerializeField] protected EquipmentEmplacement enumEquipmentEmplacement;

	void OnEnable()
	{
		enumItemType = ItemType.Equipment;
	}
}
