using UnityEngine;
using System.Collections;

public class FilterList : MonoBehaviour
{
	[SerializeField] private Transform filterContent;

	public Inventory Inventory { get { return inventory; } set { inventory = value; } }
	private Inventory inventory;

	public void AddButtonFavorite()
	{
		ButtonForInventory favorite = Instantiate(inventory.ButtonPrefab);
		favorite.transform.SetParent(filterContent);
		favorite.ChangeText("Favorite");
		favorite.button.onClick.AddListener(delegate { inventory.DisplayFavorite(); });
	}
	public ButtonForInventory AddButtonAll()
	{
		ButtonForInventory button = Instantiate(inventory.ButtonPrefab);
		button.transform.SetParent(filterContent);
		button.ChangeText("All");
		button.button.onClick.AddListener(delegate { inventory.DisplayAll(); });

		return button;
	}
	public void AddButtonFilter(Item item)
	{
		ButtonForInventory otherButton = Instantiate(inventory.ButtonPrefab);
		otherButton.transform.SetParent(filterContent);
		otherButton.ChangeText(item.EnumItemType.ToString());
		otherButton.button.onClick.AddListener(delegate { inventory.DisplayItems(item.EnumItemType); });
	}

	public void ClearList()
	{
		for (int idx = filterContent.childCount - 1; idx >= 0; --idx)
			Destroy(filterContent.GetChild(idx).gameObject);
	}
}
