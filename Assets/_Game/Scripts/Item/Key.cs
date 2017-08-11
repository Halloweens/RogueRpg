using UnityEngine;

[CreateAssetMenu(menuName = "Item/Key")]
public class Key : Item
{
	void OnEnable()
	{
		enumItemType = ItemType.Key;
	}
}
