using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Item/List/List")]
public class ItemsList : ScriptableObject
{
	public List<Item> AllItems { get { return allItems;} }
	[SerializeField] private List<Item> allItems = new List<Item>();
}
