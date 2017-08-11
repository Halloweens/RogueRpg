using UnityEngine;
using UnityEngine.UI;

public class SpellList : MonoBehaviour
{
	[SerializeField] private Transform content;

	public MagicInterface MagicInterface { get { return magicInterface; } set { magicInterface = value; } }
	private MagicInterface magicInterface;

	void Awake()
	{
		magicInterface = transform.parent.GetComponent<MagicInterface>();
	}

	public void AddButtonSpell(SpellData spell)
	{
        if (!magicInterface)
            magicInterface = transform.parent.GetComponent<MagicInterface>();

        ButtonForInventory button = Instantiate(magicInterface.ButtonPrefab);
		button.transform.SetParent(content);
		button.ChangeText(spell.SpellName);

		if (magicInterface.FavoriteSpells.Contains(spell))
			button.ChangeTextColor(magicInterface.ColorOfFavorite);

		button.button.onClick.AddListener(delegate { magicInterface.DisplaySpellCharacteristics(spell); });

		if (magicInterface.Player.Arsenal.HasSpell(spell) != -1)
			button.DisplayImageEquipped();
	}

	public void ClearList()
	{
		for (int idx = content.childCount - 1; idx >= 0; --idx)
			Destroy(content.GetChild(idx).gameObject);
	}

	public ButtonForInventory FindButtonWithText(string text)
	{
		for (int idx = 0; idx < content.childCount; ++idx)
			if (content.GetChild(idx).GetComponentInChildren<Text>().text == text)
				return content.GetChild(idx).GetComponent<ButtonForInventory>();
		return null;
	}
}
