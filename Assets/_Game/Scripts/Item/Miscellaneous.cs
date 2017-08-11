using UnityEngine;

[CreateAssetMenu(menuName = "Item/Miscellaneous")]
public class Miscellaneous : Item
{
	void OnEnable()
	{
		enumItemType = ItemType.Miscellaneous;
	}
}
