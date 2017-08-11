using UnityEngine;
using UnityEngine.UI;

public class ItemCharacteristics : MonoBehaviour
{
	[SerializeField] private Text nameLabel;
	[SerializeField] private Text particularLabel;
	[SerializeField] private Text weightLabel;
	[SerializeField] private Text valueLabel;
	[SerializeField] private Text descriptionLabel;

	[SerializeField] private RawImage image;

	private string weightText;
	private string valueText;

	void Awake()
	{
		weightText = weightLabel.text;
		valueText = valueLabel.text;

		Hide();
	}

	public void Display(Item item, uint nb_item)
	{
		gameObject.SetActive(true);

		nameLabel.text = nb_item > 1 ? item.ItemName.ToUpper() + " (" + nb_item + ")" : item.ItemName.ToUpper();

		if (item.EnumItemType == Item.ItemType.Weapon || item.EnumItemType == Item.ItemType.Equipment)
		{
			if (item.EnumItemType == Item.ItemType.Weapon)
				particularLabel.text = "Damages : " + ((WeaponData)item).Weapon.damages;
			else
				particularLabel.text = "Armor : " + ((Equipment)item).Armor;
		}
		else
			particularLabel.text = "";
		if (item.EnumItemType == Item.ItemType.Consommable || item.EnumItemType == Item.ItemType.QuestItem)
			descriptionLabel.text = item.EnumItemType == Item.ItemType.Consommable ? ((Consommable)item).Description : ((QuestItem)item).Description;
		else
			descriptionLabel.text = "";

		weightLabel.text = string.Format(weightText, item.Weight);
		valueLabel.text = string.Format(valueText, item.ItemValue);
		image.texture = item.Icon;
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
