using UnityEngine;
using System.Collections;

public class SpellData : ScriptableObject
{
    public enum SpellAnimationType : int
    {
        Target = 0,
        Self = 1
    }

	public enum SpellType : int
	{
		Destruction = 0,
		Healing
	}

	public enum SpellUsing : int
	{
		OneShot = 0,
		Continued
	}

    public Spell Spell { get { return spell; } }
    [SerializeField] private Spell spell = null;

	public SpellType EnumSpellType { get { return spellType; } private set { spellType = value; } }
	protected SpellType spellType;

    public SpellUsing SpellUsingType { get { return spellUsingType; } private set { spellUsingType = value; } }
    [SerializeField] protected SpellUsing spellUsingType;

    public SpellAnimationType AnimationType { get { return animationType; } private set { animationType = value; } }
    [SerializeField] private SpellAnimationType animationType = SpellAnimationType.Self;

	public string SpellName { get { return spellName; } private set { spellName = value; } }
	[SerializeField] protected string spellName;

	public uint Cost { get { return cost; } private set { cost = value; } }
	[SerializeField] protected uint cost;

	public string Description { get { return description; } private set { description = value; } }
	[SerializeField] protected string description;

	public Texture2D Icon { get { return icon; } protected set { icon = value; } }
	[SerializeField] protected Texture2D icon;
}

public class SpellComparer : System.Collections.Generic.IEqualityComparer<SpellData>
{
	public bool Equals(SpellData spell1, SpellData spell2)
	{
		if (spell1.EnumSpellType == spell2.EnumSpellType && spell1.SpellName == spell2.SpellName && spell1.Cost == spell2.Cost && spell1.Description == spell2.Description)
			return true;
		return false;
	}

	public int GetHashCode(SpellData spell)
	{
		string code = spell.EnumSpellType.ToString() + "|" + spell.SpellName + "|" + spell.Cost.ToString() + "|" + spell.Description;
		return code.GetHashCode();
	}
}
