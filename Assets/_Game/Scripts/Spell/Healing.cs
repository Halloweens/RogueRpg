using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Spell/Healing")]
public class Healing : SpellData
{
	void OnEnable()
	{
		spellType = SpellType.Healing;
	}
}
