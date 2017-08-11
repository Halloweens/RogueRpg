using UnityEngine;
using UnityEngine.UI;

public class MagicLowerBar : MonoBehaviour
{
	[SerializeField] private Text equip;
	[SerializeField] private Button favorite;

	public MagicInterface MagicInterface { get { return magicInterface; } set { magicInterface = value; } }
	private MagicInterface magicInterface;

	void Awake()
	{
		magicInterface = transform.parent.GetComponent<MagicInterface>();
	}

	void Update()
	{
		if (Input.anyKeyDown)
		{
			ManageEquip();
		}
	}

	private void ManageEquip()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			EquipSpell(0);
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			EquipSpell(1);
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			EquipSpell(2);
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			EquipSpell(3);
		else if (Input.GetKeyDown(KeyCode.Alpha5))
			EquipSpell(4);
		else if (Input.GetKeyDown(KeyCode.Alpha6))
			EquipSpell(5);
		else if (Input.GetKeyDown(KeyCode.Alpha7))
			EquipSpell(6);
		else if (Input.GetKeyDown(KeyCode.Alpha8))
			EquipSpell(7);
		else if (Input.GetKeyDown(KeyCode.Alpha9))
			EquipSpell(8);
	}

	public void ClickOnFavorite()
	{
		ButtonForInventory button = magicInterface.GetSelectedButton();

		if (magicInterface.FavoriteSpells.Contains(magicInterface.SelectedSpell))
		{
			magicInterface.FavoriteSpells.Remove(magicInterface.SelectedSpell);

			if (magicInterface.FavoriteDisplayed)
			{
				Destroy(button.gameObject);

				magicInterface.Unselect();
			}
			else
			{
				favorite.GetComponentInChildren<Text>().text = "Favorite";
				button.InitTextColor();
			}

			return;
		}

		magicInterface.FavoriteSpells.Add(magicInterface.SelectedSpell);
		button.ChangeTextColor(magicInterface.ColorOfFavorite);
		favorite.GetComponentInChildren<Text>().text = "Unfavorite";
	}

	public void EquipSpell(int idx)
	{
		ButtonForInventory button = magicInterface.GetSelectedButton();
		int equippedInIdx = magicInterface.Player.Arsenal.HasSpell(magicInterface.SelectedSpell);

		if (equippedInIdx != -1)
		{
			if (idx != equippedInIdx)
			{
				magicInterface.Player.Arsenal.UnequipSpell(magicInterface.SelectedSpell, equippedInIdx);
				magicInterface.Player.Arsenal.EquipSpell(magicInterface.SelectedSpell, idx);
			}
			else
			{
				magicInterface.Player.Arsenal.UnequipSpell(magicInterface.SelectedSpell, idx);
				button.HideImageEquipped();
				TextEquipOrUnequip(false);
			}
		}
		else
		{
			magicInterface.Player.Arsenal.EquipSpell(magicInterface.SelectedSpell, idx);
			button.DisplayImageEquipped();
			TextEquipOrUnequip(true);
		}

		magicInterface.EquippedSpellPanel.RefreshList();
	}

	public void TextEquipOrUnequip(bool spellIsEquipped)
	{
		equip.text = spellIsEquipped ? "[1 - 9] Unequip / Equip" : "[1 - 9] Equip";
	}

	public void SpellFavorite(bool isFavorite)
	{
		favorite.GetComponentInChildren<Text>().text = isFavorite ? "Unfavorite" : "Favorite";
	}

	public void DisplayButtons(bool display)
	{
		equip.gameObject.SetActive(display);
		favorite.gameObject.SetActive(display);

		if (display)
		{
			TextEquipOrUnequip(magicInterface.Player.Arsenal.HasSpell(magicInterface.SelectedSpell) != -1 ? true : false);
			SpellFavorite(magicInterface.FavoriteSpells.Contains(magicInterface.SelectedSpell));
		}
	}
}
