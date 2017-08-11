using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Arsenal : MonoBehaviour
{
	private Player player;
	private InventoryToSearch inventoryToSearch;

    public Equipment HeadArmor { get { return headArmor; } set { headArmor = value; RefreshTotalArmor(); } }
    [SerializeField] private Equipment headArmor;

    public Equipment ChestArmor { get { return chestArmor; } set { chestArmor = value; RefreshTotalArmor(); } }
    [SerializeField] private Equipment chestArmor;

    public Equipment FeetsArmor { get { return feetsArmor; } set { feetsArmor = value; RefreshTotalArmor(); } }
    [SerializeField] private Equipment feetsArmor;

    public WeaponData RightHandWeapon { get { return rightHandWeapon; } set
        {
            ClearWeaponSocket(WeaponHandSocket.Right);

            rightHandWeapon = value;

            if (rightHandWeapon != null)
                InitializeWeaponSocket(WeaponHandSocket.Right, rightHandWeapon);

            RefreshTotalArmor();
        }
    }
    [SerializeField] private WeaponData rightHandWeapon;

    public WeaponData LeftHandWeapon { get { return leftHandWeapon; } set
        {
            ClearWeaponSocket(WeaponHandSocket.Left);

            leftHandWeapon = value;

            if (leftHandWeapon != null)
                InitializeWeaponSocket(WeaponHandSocket.Left, leftHandWeapon);

            RefreshTotalArmor();
        }
    }
    [SerializeField] private WeaponData leftHandWeapon;

    public Equipment Shield { get { return shield; } set { shield = value; RefreshTotalArmor(); } }
    [SerializeField] private Equipment shield;

    public Transform rightHandSocket = null;
    public Transform leftHandSocket = null;

    public Weapon RightHandWeaponInstance { get { return rightHandWeaponInstance; } }
    private Weapon rightHandWeaponInstance = null;

    public Weapon LeftHandWeaponInstance { get { return leftHandWeaponInstance; } }
    private Weapon leftHandWeaponInstance = null;

	[System.NonSerialized] public SpellData[] spells = new SpellData[9];

    public UnityEvent onWeaponsChanged = new UnityEvent();

	public uint TotalArmor { get { return totalArmor; } private set { totalArmor = value; } }
	private uint totalArmor = 0;

	void Start()
	{
		player = GetComponent<Player>();
		inventoryToSearch = GetComponent<InventoryToSearch>();

		AddInInventory(headArmor);
		AddInInventory(chestArmor);
		AddInInventory(feetsArmor);
		AddInInventory(rightHandWeapon);
		AddInInventory(leftHandWeapon);

		AddInInventory(shield);

		if (rightHandSocket == null || leftHandSocket == null)
		{
			Debug.LogWarning(@"RightHandSocket or LeftHandSocket aren't correctly configured.
Undefined bahavior can happen when equipping weapons. Please check your settings.");
		}

        Damageable damageable = GetComponent<Damageable>();

        if (damageable != null)
            damageable.onDamageTaken.AddListener(ApplyArmorOnDamage);
	}

	private void AddInInventory(Item item)
	{
		if (item)
		{
            if (player && !player.PlayerInventory.Items.ContainsKey(item))
            {
                player.PlayerInventory.AddItem(item);
                if (item is WeaponData)
                {
                    if ((item as WeaponData).HandSocket == WeaponHandSocket.Right)
                    {
                        if (rightHandWeapon)
                        {
                            ClearWeaponSocket(WeaponHandSocket.Right);
                            InitializeWeaponSocket(WeaponHandSocket.Right, rightHandWeapon);
                        }
                    }
                    else
                    {
                        if (leftHandWeapon)
                        {
                            ClearWeaponSocket(WeaponHandSocket.Left);
                            InitializeWeaponSocket(WeaponHandSocket.Left, leftHandWeapon);
                        }
                    }
                }
            }
            else if (!player)
            {
                inventoryToSearch.AddItem(item);
                if (item is WeaponData)
                {
                    if ((item as WeaponData).HandSocket == WeaponHandSocket.Right)
                    {
                        if (rightHandWeapon)
                        {
                            ClearWeaponSocket(WeaponHandSocket.Right);
                            InitializeWeaponSocket(WeaponHandSocket.Right, rightHandWeapon);
                        }
                    }
                    else
                    {
                        if (leftHandWeapon)
                        {
                            ClearWeaponSocket(WeaponHandSocket.Left);
                            InitializeWeaponSocket(WeaponHandSocket.Left, leftHandWeapon);
                        }
                    }
                }
            }
        }
	}

	public bool HasItem(Item item)
	{
		if (item.EnumItemType == Item.ItemType.Equipment)
		{
			Equipment equipment = (Equipment)item;

			if (equipment == headArmor || equipment == chestArmor || equipment == feetsArmor || equipment == shield)
				return true;
			return false;
		}
		else if (item.EnumItemType == Item.ItemType.Weapon && ((WeaponData)item == leftHandWeapon || (WeaponData)item == rightHandWeapon))
			return true;

		return false;
	}

	public Item Equip(Item item)
	{
		Item itemToReturn = null;

		if (item.EnumItemType == Item.ItemType.Equipment)
		{
			Equipment equipment = (Equipment)item;

			if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Head)
			{
				itemToReturn = headArmor;
				headArmor = equipment;
			}
			else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Chest)
			{
				itemToReturn = chestArmor;
				chestArmor = equipment;
			}
			else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Feet)
			{
				itemToReturn = feetsArmor;
				feetsArmor = equipment;
			}
			else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Shield)
			{
				itemToReturn = shield;
				shield = equipment;
			}
		}
		else if (item.EnumItemType == Item.ItemType.Weapon)
		{
            WeaponData weapon = item as WeaponData;
            if (weapon.HandSocket == WeaponHandSocket.Left)
            {
                itemToReturn = leftHandWeapon;
                leftHandWeapon = weapon;
                ClearWeaponSocket(WeaponHandSocket.Left);
                InitializeWeaponSocket(WeaponHandSocket.Left, leftHandWeapon);
            }
            else if (weapon.HandSocket == WeaponHandSocket.Right)
            {
                itemToReturn = rightHandWeapon;
                rightHandWeapon = weapon;
                ClearWeaponSocket(WeaponHandSocket.Right);
                InitializeWeaponSocket(WeaponHandSocket.Right, rightHandWeapon);
            }
        }

		RefreshTotalArmor();
		return itemToReturn;
	}

	public void Unequip(Item item)
	{
        if (item.EnumItemType == Item.ItemType.Equipment)
        {
            Equipment equipment = (Equipment)item;

            if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Head)
                headArmor = null;
            else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Chest)
                chestArmor = null;
            else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Feet)
                feetsArmor = null;
            else if (equipment.EnumEquipmentEmplacement == Equipment.EquipmentEmplacement.Shield)
                shield = null;
        }
        else if (item.EnumItemType == Item.ItemType.Weapon && (item as WeaponData).HandSocket == WeaponHandSocket.Left)
        {
            ClearWeaponSocket(WeaponHandSocket.Left);
            leftHandWeapon = null;
        }
        else if (item.EnumItemType == Item.ItemType.Weapon && (item as WeaponData).HandSocket == WeaponHandSocket.Right)
        {
            ClearWeaponSocket(WeaponHandSocket.Right);
            rightHandWeapon = null;
        }
		RefreshTotalArmor();
	}

	private void RefreshTotalArmor()
	{
		uint curArmor = 0;
		if (headArmor)
			curArmor += headArmor.Armor;
		if (chestArmor)
			curArmor += chestArmor.Armor;
		if (feetsArmor)
			curArmor += feetsArmor.Armor;
		if (shield)
			curArmor += shield.Armor;

		totalArmor = curArmor;
	}

    public void ClearWeaponSocket(WeaponHandSocket socketType)
    {
        Transform socket = socketType == WeaponHandSocket.Left ? leftHandSocket : rightHandSocket;

        foreach (Transform t in socket)
            Destroy(t.gameObject);

        if (socketType == WeaponHandSocket.Left)
        {
            if (leftHandWeaponInstance != null)
                Destroy(leftHandWeaponInstance.gameObject);
            leftHandWeaponInstance = null;
        }
        else
        {
            if (rightHandWeaponInstance != null)
                Destroy(rightHandWeaponInstance.gameObject);
            rightHandWeaponInstance = null;
        }
    }

    public void InitializeWeaponSocket(WeaponHandSocket socketType, WeaponData weaponData)
    {
        Transform socket = socketType == WeaponHandSocket.Left ? leftHandSocket : rightHandSocket;

        if (weaponData.Weapon != null)
        {
            Weapon weapon = Instantiate(weaponData.Weapon) as Weapon;
            weapon.transform.SetParent(socket, false);
            weapon.ignoredlayers = 1 << gameObject.layer;

            if (socketType == WeaponHandSocket.Left)
                leftHandWeaponInstance = weapon;
            else
                rightHandWeaponInstance = weapon;

            if (onWeaponsChanged != null)
                onWeaponsChanged.Invoke();
        }
        else
            Debug.LogWarning("No weapon defined for " + weaponData.name);
    }

	public SpellData EquipSpell(SpellData spell, int idx)
	{
		SpellData spellToReturn = spells[idx];
		spells[idx] = spell;
		return spellToReturn;
	}

	public void UnequipSpell(SpellData spell, int idx)
	{
		spells[idx] = null;
	}

	public int HasSpell(SpellData spell)
	{
		for (int idx = 0; idx < spells.Length; ++idx)
			if (spells[idx] == spell)
				return idx;
		return -1;
	}

    private void ApplyArmorOnDamage(OnDamageTakenArgs args)
    {
        float reducedPercentage = (totalArmor / (totalArmor + 600.0f));

        if (reducedPercentage > 0.8f)
            reducedPercentage = 0.8f;

        args.damageAmount.Value *= (1.0f - reducedPercentage);
    }
}
