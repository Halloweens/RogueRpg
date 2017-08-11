using UnityEngine;
using UnityEngine.SceneManagement;

public class ProceduralMapRoot : MonoBehaviour
{
	[SerializeField] private DungeonGenerator dungeonGenerator;

	private bool loadNeeded = false;

	private int currentLevel = 0;

	void Start ()
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("ProceduralMap"));
		if (SceneManager.GetSceneByName("StartZone").isLoaded)
		{
			SceneManager.UnloadScene("StartZone");
		}
		if (SceneManager.GetSceneByName("BetweenLevelsScene").isLoaded)
			loadNeeded = true;

		transform.GetChild(0).GetComponent<EnemiesGenerator>().onAllEnemiesCreated += LevelUpEnemies;
	}

	void LateUpdate()
	{
		if (loadNeeded)
			Load();
	}

	private void Load()
	{
		BetweenLevelsData bld = SceneManager.GetSceneByName("BetweenLevelsScene").GetRootGameObjects()[0].GetComponent<BetweenLevelsData>();

		if (!GameObject.FindGameObjectWithTag("Player"))
			return;

		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		player.PlayerInventory.Items = bld.playerInventoryItems;
		player.PlayerInventory.Gold = bld.playerGold;
        player.GetComponent<Entity>().entityName = bld.playerName;
        player.GetComponent<Damageable>().Hp = bld.hp;
		player.GetComponent<ManaManager>().Mana = bld.mana;
        player.Characteristics.RemainingPoints = bld.playerCharacteristics.remaining;
        player.Characteristics.Experience = bld.xp;
        player.Characteristics.TargetExp = bld.playerCharacteristics.targetXP;
		player.Characteristics.Level = bld.level;

		player.Characteristics.Strength = bld.playerCharacteristics.strength;
		player.Characteristics.Constitution = bld.playerCharacteristics.constitution;
		player.Characteristics.Intelligence = bld.playerCharacteristics.intelligence;
		player.Characteristics.Dexterity = bld.playerCharacteristics.dexterity;

        player.Arsenal.HeadArmor = bld.playerArsenal.headArmor;
		player.Arsenal.ChestArmor = bld.playerArsenal.chestArmor;
        player.Arsenal.FeetsArmor = bld.playerArsenal.feetsArmor;
		player.PlayerInventory.RefreshWeight();

		player.Arsenal.RightHandWeapon = bld.playerArsenal.rightHandWeapon;

        player.Arsenal.LeftHandWeapon = bld.playerArsenal.leftHandWeapon;

        player.Arsenal.Shield = bld.playerArsenal.shield;

        player.Arsenal.spells = bld.spells;

		if (bld.timeSinceFirstStart != 0f)
			TimeManager.Instance.TimeSinceFirstStart = bld.timeSinceFirstStart;

		++bld.curLevel;
		currentLevel = bld.curLevel;
		loadNeeded = false;

		if (bld.curLevel >= bld.lastLevel)
		{
			dungeonGenerator.bossCreated += BossCreated;
			dungeonGenerator.SpawnBoss();
			dungeonGenerator.portalCreated += DestroyPortal;
		}
	}

	private void LevelUpEnemies()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject enemy in enemies)
			enemy.GetComponent<Characteristics>().ForceLevelUpToValue(currentLevel);
	}

	private void BossCreated()
	{
		if (UIRoot.Instance.Compass.Initialized)
			UIRoot.Instance.Compass.AddPointOnCompass(GameObject.FindGameObjectWithTag("Boss"), Color.red);
		else
			UIRoot.Instance.Compass.compassInitialized += BossCreated;
	}

	private void DestroyPortal()
	{
		Destroy(GameObject.FindGameObjectWithTag("PortalToNextLevel"));
	}
}
