using UnityEngine;

[CreateAssetMenu(menuName ="Item/Quest Item")]
public class QuestItem : Item
{
	public string Description { get { return description; } }
	[SerializeField] private string description;

	void OnEnable()
	{
		enumItemType = ItemType.QuestItem;
	}
}
