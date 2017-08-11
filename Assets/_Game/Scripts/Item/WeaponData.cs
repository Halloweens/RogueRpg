using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon")]
public class WeaponData : Item
{
    public WeaponHandSocket HandSocket { get { return handSocket; } }
    [SerializeField]
    private WeaponHandSocket handSocket = WeaponHandSocket.Right;

    public Weapon Weapon { get { return weapon; } }
    [SerializeField]
    private Weapon weapon = null;

    /// <summary>
    /// Will be used later on by the CharController to know which animation it should be playing, not used atm since we only have a sword.
    /// </summary>
    public WeaponType Type { get { return type; } }
    public WeaponType type = WeaponType.Sword;

	void OnEnable()
	{
		enumItemType = ItemType.Weapon;
	}
}

public enum WeaponHandSocket
{
    Left,
    Right
}

public enum WeaponType
{
    Sword,
    Shield,
    Other
}