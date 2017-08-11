using UnityEngine;
using System.Collections;

public class MagicFilterList : MonoBehaviour
{
	[SerializeField] private Transform content;

	public MagicInterface MagicInterface { get { return magicInterface; } set { magicInterface = value; } }
	private MagicInterface magicInterface;

	void Awake()
	{
		magicInterface = transform.parent.GetComponent<MagicInterface>();
	}

	public void AddButtonFavorite()
	{
		ButtonForInventory favorite = Instantiate(magicInterface.ButtonPrefab);
		favorite.transform.SetParent(content);
		favorite.ChangeText("Favorite");
		favorite.button.onClick.AddListener(delegate { magicInterface.DisplayFavorite(); });
	}
	public ButtonForInventory AddButtonAll()
	{
		ButtonForInventory button = Instantiate(magicInterface.ButtonPrefab);
		button.transform.SetParent(content);
		button.ChangeText("All");
		button.button.onClick.AddListener(delegate { magicInterface.DisplayAll(); });

		return button;
	}
	public void AddButtonFilter(SpellData spell)
	{
		ButtonForInventory otherButton = Instantiate(magicInterface.ButtonPrefab);
		otherButton.transform.SetParent(content);
		otherButton.ChangeText(spell.EnumSpellType.ToString());
		otherButton.button.onClick.AddListener(delegate { magicInterface.DisplaySpells(spell.EnumSpellType); });
	}

	public void ClearList()
	{
		for (int idx = content.childCount - 1; idx >= 0; --idx)
			Destroy(content.GetChild(idx).gameObject);
	}

}
