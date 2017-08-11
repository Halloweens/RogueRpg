using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consommable")]
public class Consommable : Item
{
	public enum Affectation
	{
		Health = 0,
		Mana,
		Teleportation
	}

	public Affectation EnumAffectation { get { return affectation; } set { affectation = value; } }
	[SerializeField] private Affectation affectation;

	public int BonusValue { get { return bonusValue; } set { bonusValue = value; } }
	[SerializeField] private int bonusValue;

	public string Description { get { return description; } set { description = value; } }
	[SerializeField] private string description = "";

	public string TeleportationSceneName { get { return teleportationSceneName; } set { teleportationSceneName = value; } }
	[SerializeField] private string teleportationSceneName = "";

	void OnEnable()
	{
		enumItemType = ItemType.Consommable;
	}
}
