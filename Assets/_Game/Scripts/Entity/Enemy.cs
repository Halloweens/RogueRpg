using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Characteristics))]
[RequireComponent(typeof(Damageable))]
public class Enemy : MonoBehaviour
{
    public int xp;
	public ItemsList AllItems { get { return allItems; } set { allItems = value; } }
	protected ItemsList allItems;

	[SerializeField] protected uint nbMaxItemsToCarry;

	private Damageable damageable;
	private Characteristics characteristics;

	private bool canBeDestroyed = false;

	void Start()
	{
		damageable = GetComponent<Damageable>();
		characteristics = GetComponent<Characteristics>();
		damageable.onDeath.AddListener(OnEnemyDeath);
		characteristics.onStatsChanged.AddListener(CheckLevelUp);
		CheckLevelUp();
	}

	protected Item randomItem()
	{
		return allItems.AllItems[Random.Range(0, allItems.AllItems.Count)];
	}

	private void OnEnemyDeath(OnDeathArgs args)
	{
        Characteristics c = args.source.GetComponent<Characteristics>();
        if (c != null)
            c.Experience += xp;

        StartCoroutine("CheckInventoryEmpty");
	}

	private void CheckLevelUp()
	{
		if (characteristics.RemainingPoints != 0)
		{
			characteristics.SetPointsOnRandomStat();
			damageable.RefreshHealthStats(characteristics.Constitution);
			xp = 50 * characteristics.Level;
		}
	}

	public IEnumerator CheckInventoryEmpty()
	{
		if (GetComponent<InventoryToSearch>())
		{
			InventoryToSearch inv = GetComponent<InventoryToSearch>();
			yield return new WaitForSeconds(5f);

			while (inv.Items.Count != 0)
			{
				yield return new WaitForSeconds(5f);
			}
			canBeDestroyed = true;
			if (!UIRoot.Instance.UIRootNonPlayer.Displayed)
				Destroy(gameObject);
			else
				UIRoot.Instance.UIRootNonPlayer.inventoryDisplayChange += DestroyEnemy;
		}
	}

	private void DestroyEnemy()
	{
		if (!UIRoot.Instance.UIRootNonPlayer.Displayed)
			if (canBeDestroyed)
			    Destroy(gameObject);
	}
}
