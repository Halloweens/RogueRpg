using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartZoneRoot : MonoBehaviour
{
    void Start()
	{
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartZone"));
		TimeManager.Instance.TimeSinceFirstStart = 0f;
        Load();
    }
	private void Load()
	{
        BetweenLevelsData bld = SceneManager.GetSceneByName("BetweenLevelsScene").GetRootGameObjects()[0].GetComponent<BetweenLevelsData>();

		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.GetComponent<Entity>().entityName = bld.playerName;
		player.GetComponent<Damageable>().Hp = bld.hp;
		player.GetComponent<ManaManager>().Mana = bld.mana;
		player.Characteristics.Level = bld.level;
        player.Characteristics.TargetExp = bld.playerCharacteristics.targetXP;
        player.Characteristics.Experience = bld.xp;

		player.Characteristics.Strength = bld.playerCharacteristics.strength;
		player.Characteristics.Constitution = bld.playerCharacteristics.constitution;
		player.Characteristics.Intelligence = bld.playerCharacteristics.intelligence;
		player.Characteristics.Dexterity = bld.playerCharacteristics.dexterity;

		if (bld.curLevel == 0)
			return;
		else
			--bld.curLevel;

		player.Arsenal.HeadArmor = bld.playerArsenal.headArmor;
		player.Arsenal.ChestArmor = bld.playerArsenal.chestArmor;
		player.Arsenal.FeetsArmor = bld.playerArsenal.feetsArmor;
		player.PlayerInventory.Items = bld.playerInventoryItems;
		player.PlayerInventory.RefreshWeight();
		player.PlayerInventory.Gold = bld.playerGold;

		player.Arsenal.RightHandWeapon = bld.playerArsenal.rightHandWeapon;
		if (bld.playerArsenal.rightHandWeapon)
			player.Arsenal.InitializeWeaponSocket(WeaponHandSocket.Right, player.Arsenal.RightHandWeapon);

		player.Arsenal.LeftHandWeapon = bld.playerArsenal.leftHandWeapon;
		if (bld.playerArsenal.leftHandWeapon)
			player.Arsenal.InitializeWeaponSocket(WeaponHandSocket.Left, player.Arsenal.LeftHandWeapon);

		player.Arsenal.Shield = bld.playerArsenal.shield;

		player.Arsenal.spells = bld.spells;

		if (bld.timeSinceFirstStart != 0f)
			TimeManager.Instance.TimeSinceFirstStart = bld.timeSinceFirstStart;
    }
}
