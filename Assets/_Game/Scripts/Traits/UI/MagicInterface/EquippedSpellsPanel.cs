using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSpellsPanel : MonoBehaviour
{
	public MagicInterface MagicInterface { get { return magicInterface; } set { magicInterface = value; } }
	private MagicInterface magicInterface;

	private List<Text> texts = new List<Text>();

	void Awake()
	{
		magicInterface = transform.parent.GetComponent<MagicInterface>();
		foreach (Transform child in transform)
			texts.Add(child.GetComponent<Text>());
	}

	public void RefreshList()
	{
		SpellData[] spells = magicInterface.Player.Arsenal.spells;

		for (int idx = 0; idx < spells.Length; ++idx)
		{
			if (spells[idx])
				texts[idx].text = spells[idx].SpellName;
			else
				texts[idx].text = "";
		}
	}
}
