using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Usable))]
public class PortalToOtherLevel : MonoBehaviour
{
	void Start ()
	{
		GetComponent<Usable>().onUsable.AddListener(OnUseableCallback);
		UIRoot.Instance.Compass.AddPointOnCompass(gameObject, Color.magenta);
	}

	void OnUseableCallback(OnUsableArg arg)
	{
		Save();
		UnityEngine.EventSystems.EventSystem.current.enabled = false;
		SceneManager.LoadScene("ProceduralMap", LoadSceneMode.Additive);
		SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void OnDestroy()
	{
		UIRoot.Instance.Compass.RemovePoint(gameObject);
	}

	private void Save()
	{
		BetweenLevelsData bld = SceneManager.GetSceneByName("BetweenLevelsScene").GetRootGameObjects()[0].GetComponent<BetweenLevelsData>();

		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		bld.playerInventoryItems = player.PlayerInventory.Items;
		bld.playerGold = player.PlayerInventory.Gold;
        bld.playerName = player.GetComponent<Entity>().entityName;
		bld.hp = player.GetComponent<Damageable>().Hp;
		bld.mana = player.GetComponent<ManaManager>().Mana;
        bld.xp = player.Characteristics.Experience;
        bld.playerCharacteristics.remaining = player.Characteristics.RemainingPoints;
        bld.playerCharacteristics.targetXP = player.Characteristics.TargetExp;
		bld.level = player.Characteristics.Level;

		bld.playerCharacteristics.strength = player.Characteristics.Strength;
		bld.playerCharacteristics.constitution = player.Characteristics.Constitution;
		bld.playerCharacteristics.intelligence = player.Characteristics.Intelligence;
		bld.playerCharacteristics.dexterity = player.Characteristics.Dexterity;

		bld.playerArsenal.headArmor = player.Arsenal.HeadArmor;
		bld.playerArsenal.chestArmor = player.Arsenal.ChestArmor;
		bld.playerArsenal.feetsArmor = player.Arsenal.FeetsArmor;
		bld.playerArsenal.rightHandWeapon = player.Arsenal.RightHandWeapon;
		bld.playerArsenal.leftHandWeapon = player.Arsenal.LeftHandWeapon;
		bld.playerArsenal.shield = player.Arsenal.Shield;

		bld.spells = player.Arsenal.spells;

		bld.timeSinceFirstStart = TimeManager.Instance.TimeSinceFirstStart;
	}
}
