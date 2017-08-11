using UnityEngine;

[CreateAssetMenu(menuName = "Spell/Destruction")]
public class Destruction : SpellData
{
	void OnEnable()
	{
		spellType = SpellType.Destruction;
	}
}
