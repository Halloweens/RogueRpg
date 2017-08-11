using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Characteristics))]
[RequireComponent(typeof(Arsenal))]
[RequireComponent(typeof(Damageable))]
public class Player : MonoBehaviour
{
    public Transform CameraFocus { get { return cameraFocus; } }
    [SerializeField]
    private Transform cameraFocus = null;

	public Inventory PlayerInventory { get { return inventory; } set { inventory = value; SetInventoryType(); } }
	private Inventory inventory;

    public Entity Entity {get { return entity; } set { entity = value; } }
    private Entity entity = null;

	public Characteristics Characteristics { get { return characteristics; } private set { characteristics = value; } }
    private Characteristics characteristics = null;

	public ManaManager ManaMgr { get { return manaMgr; } }
	private ManaManager manaMgr = null;

    public Arsenal Arsenal { get { return arsenal; } }
    private Arsenal arsenal = null;

    public Damageable Damageable { get { return damageable; } }
    private Damageable damageable = null;

    public LevelManager LvlMng { get { return lvlMng; } }
    private LevelManager lvlMng = null;

	void Awake ()
	{
        characteristics = GetComponent<Characteristics>();
		manaMgr = GetComponent<ManaManager>();
        arsenal = GetComponent<Arsenal>();
        damageable = GetComponent<Damageable>();
        lvlMng = GetComponent<LevelManager>();

		characteristics.onStatsChanged.AddListener(UpdateHealthStats);
	}
	
	private void SetInventoryType()
	{
		inventory.MaxWeight = 50.0f + characteristics.Strength * 2.0f;
		inventory.InventoryType = Inventory.InterfaceType.PlayerInventory;
		inventory.Player = this;
	}

	private void UpdateHealthStats()
	{
		damageable.RefreshHealthStats(characteristics.Constitution);
	}
}
