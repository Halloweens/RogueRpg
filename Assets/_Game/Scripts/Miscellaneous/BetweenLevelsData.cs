using UnityEngine;
using System.Collections.Generic;

public class BetweenLevelsData : MonoBehaviour
{
	public int lastLevel = 5;

	public int curLevel = 0;
	public Dictionary<Item, uint> playerInventoryItems = new Dictionary<Item, uint>(new ItemComparer());
	public uint playerGold = 0;

	public float hp;
	public float mana;
    public int xp;
    public int level;
    public string playerName;
	public double timeSinceFirstStart = 0f;

	public PlayerCharacteristics playerCharacteristics;
	public PlayerArsenal playerArsenal;

	public SpellData[] spells = new SpellData[9];

	public struct PlayerCharacteristics
	{
		public int strength;
		public int constitution;
		public int intelligence;
		public int dexterity;
        public int targetXP;
        public int remaining;
	}

	public struct PlayerArsenal
	{
		public Equipment headArmor;
		public Equipment chestArmor;
		public Equipment feetsArmor;
		public WeaponData rightHandWeapon;
		public WeaponData leftHandWeapon;
		public Equipment shield;
	}
}
