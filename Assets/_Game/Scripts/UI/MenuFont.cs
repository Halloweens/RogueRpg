using UnityEngine;
using UnityEngine.UI;

public class MenuFont : MonoBehaviour
{
	[SerializeField] private Button skills;
	[SerializeField] private Button items;
	[SerializeField] private Button map;
	[SerializeField] private Button magic;

	public Player Player { get { return player; } set { player = value; AddListenerOnButton(); } }
	private Player player;

	void Start()
	{
	}

	public void AddListenerOnButton()
	{
		MenuPause menuInGame = transform.parent.GetComponent<MenuPause>();
		items.onClick.AddListener(delegate { menuInGame.Inventory.DisplayInventory(player); });
		magic.onClick.AddListener(delegate { menuInGame.MagicInterface.DisplayInterface(); });
	}
}
