using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicInterface : MonoBehaviour
{
	public ButtonForInventory ButtonPrefab { get { return buttonPrefab; } private set { buttonPrefab = value; } }
	[SerializeField] private ButtonForInventory buttonPrefab;

	public Color ColorOfFavorite { get { return colorOfFavorite; } private set { colorOfFavorite = value; } }
	[SerializeField] private Color colorOfFavorite;

	[SerializeField] private MagicFilterList filterList;
	[SerializeField] private SpellList spellList;
	[SerializeField] private MagicLowerBar lowerBar;
	[SerializeField] private SpellCharacteristics spellCharacteristics;
	public EquippedSpellsPanel EquippedSpellPanel { get { return equippedSpellPanel; } private set { equippedSpellPanel = value; } }
	[SerializeField] private EquippedSpellsPanel equippedSpellPanel;

	public Player Player { get { return player; } set { player = value; } }
	private Player player;

	public SpellData SelectedSpell { get { return selectedSpell; } private set { selectedSpell = value; } }
	private SpellData selectedSpell = null;

	[SerializeField] private List<SpellData> spells = new List<SpellData>();

	public List<SpellData> FavoriteSpells { get { return favoriteSpells; } private set { favoriteSpells = value; } }
	private List<SpellData> favoriteSpells = new List<SpellData>();

	public bool FavoriteDisplayed { get { return favoriteDisplayed; } private set { favoriteDisplayed = false; } }
	private bool favoriteDisplayed = false;

	public void DisplayInterface()
	{
		gameObject.SetActive(true);

		filterList.AddButtonFavorite();
		filterList.AddButtonAll().SelectButton();
		DisplayAll();

		List<SpellData.SpellType> typeAlreadyHere = new List<SpellData.SpellType>();

		foreach (SpellData spell in spells)
		{
			if (!typeAlreadyHere.Contains(spell.EnumSpellType))
			{
				filterList.AddButtonFilter(spell);

				typeAlreadyHere.Add(spell.EnumSpellType);
			}
		}
		equippedSpellPanel.RefreshList();
	}
	public void Hide()
	{
		spellList.ClearList();
		filterList.ClearList();
		selectedSpell = null;

		gameObject.SetActive(false);
	}

	public void DisplayFavorite()
	{
		DisplayOtherList();
		favoriteDisplayed = true;

		DisplayListOfSpell(favoriteSpells);
	}
	public void DisplayAll()
	{
		DisplayOtherList();

		DisplayListOfSpell(spells);
	}
	public void DisplaySpells(SpellData.SpellType type)
	{
		DisplayOtherList();

		DisplayListOfSpell(GetSpellByType(type));
	}
	public void DisplayListOfSpell(List<SpellData> spellsToDisplay)
	{
		foreach (SpellData spell in spellsToDisplay)
			spellList.AddButtonSpell(spell);
	}
	private void DisplayOtherList()
	{
		selectedSpell = null;
		spellList.ClearList();
		spellCharacteristics.Hide();
		lowerBar.DisplayButtons(false);
		favoriteDisplayed = false;
	}

	public void DisplaySpellCharacteristics(SpellData spell)
	{
		selectedSpell = spell;
		spellCharacteristics.DisplaySpellCharacteristics(spell);
		lowerBar.DisplayButtons(true);
	}

	private List<SpellData> GetSpellByType(SpellData.SpellType type)
	{
		List<SpellData> spellsToReturn = new List<SpellData>();

		foreach (SpellData spell in spells)
			if (spell.EnumSpellType == type)
				spellsToReturn.Add(spell);

		return spellsToReturn;
	}
	public ButtonForInventory GetSelectedButton()
	{
		if (selectedSpell)
			return spellList.FindButtonWithText(selectedSpell.SpellName);
		return null;
	}

	public void Unselect()
	{
		selectedSpell = null;
		lowerBar.DisplayButtons(false);
		spellCharacteristics.Hide();
	}

	public void AddSpell(SpellData spell)
	{
		if (!spells.Contains(spell))
			spells.Add(spell);
	}
}
