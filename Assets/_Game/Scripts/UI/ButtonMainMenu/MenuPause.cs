using UnityEngine;
using System.Collections;

public class MenuPause : MonoBehaviour
{
	public MenuPlayer MenuPlayer { get { return menuPlayer; } private set { menuPlayer = value; } }
    [SerializeField] private MenuPlayer menuPlayer;
    [SerializeField] private MenuFont menu;

	public Inventory Inventory { get { return inventory; } private set { inventory = value; } }
    [SerializeField] private Inventory inventory;

	public MagicInterface MagicInterface { get { return magicInterface; } }
	[SerializeField] private MagicInterface magicInterface;
	[SerializeField] private UIRootNonPlayer uiRootNonPlayer;

	public Player Player { get { return player; } set { player = value; SetPlayer(); } }
	private Player player = null;

	public event System.Action onMenuDisplayChange;
	public bool displayed { get { return (menuPlayer.gameObject.activeSelf || menu.gameObject.activeSelf || magicInterface.gameObject.activeInHierarchy || inventory.gameObject.activeInHierarchy); } }

    void Start()
    {
		TimeManager.Instance.changePause += CheckPause;
    }

    void Update()
    {
        if (!uiRootNonPlayer.Displayed && Input.GetKeyDown(KeyCode.Escape) && menuPlayer.gameObject.activeSelf == false)
        {
			TimeManager.Instance.Pause = !TimeManager.Instance.Pause;
        }

        if (!uiRootNonPlayer.Displayed && Input.GetKeyDown(KeyCode.KeypadEnter) && menu.gameObject.activeSelf == false)
        {
			TimeManager.Instance.Pause = !TimeManager.Instance.Pause;
        }
    }



	private void SetPlayer()
	{
		magicInterface.Player = player;
		menu.Player = player;
	}

	private void CheckPause()
	{
		if (!TimeManager.Instance.Pause)
		{
			menu.gameObject.SetActive(false);
			menuPlayer.gameObject.SetActive(false);
			inventory.HideInventory();
			magicInterface.Hide();
			RaiseOnDisplayChange();
		}
		else
		{
			if (!uiRootNonPlayer.Displayed && Input.GetKeyDown(KeyCode.Escape) && menuPlayer.gameObject.activeSelf == false)
			{
				menu.gameObject.SetActive(true);
				RaiseOnDisplayChange();
			}
			if (!uiRootNonPlayer.Displayed && Input.GetKeyDown(KeyCode.KeypadEnter) && menu.gameObject.activeSelf == false)
			{
				menuPlayer.gameObject.SetActive(true);
				RaiseOnDisplayChange();
			}
		}
	}

	private void RaiseOnDisplayChange()
	{
		if (onMenuDisplayChange != null)
			onMenuDisplayChange();
	}
}