using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GrabIcon))]
public class UIRoot : MonoBehaviour
{
	public MenuPause MenuInGame { get { return menuInGame; } private set { menuInGame = value; } }
	[SerializeField] private MenuPause menuInGame;
	public UIRootNonPlayer UIRootNonPlayer { get { return uiRootNonPlayer; } private set { uiRootNonPlayer = value; } }
	[SerializeField] private UIRootNonPlayer uiRootNonPlayer;

	public Compass Compass { get { return compass; } }
	[SerializeField] private Compass compass;

    private GrabIcon grabIcon;
	[SerializeField] private GameObject grabIconUi;
	[SerializeField] private DamageableUI healthBar;
	[SerializeField] private ManaManagerUI manaBar;
    [SerializeField] private LevelManagerUI expBar;
    [SerializeField] private StatsAssignmentUI statsUI;
	[SerializeField] private Text feedbackText;

	[SerializeField] private RectTransform youDiePanel;
	[SerializeField] private RectTransform wonPanel;

    private Player player;

	public static UIRoot Instance { get { if (!instance) instance = GameObject.FindGameObjectWithTag("UIRoot").GetComponent<UIRoot>(); return instance; } }
	private static UIRoot instance;

	private bool displayFeedBackText = false;

	void Awake()
	{
        grabIcon = GetComponent<GrabIcon>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		uiRootNonPlayer.Player = player;
		menuInGame.Player = player;
		grabIcon.Player = player.gameObject;
        grabIcon.grabIconUI = grabIconUi;
		player.PlayerInventory = menuInGame.Inventory;
		healthBar.Target = player.GetComponent<Damageable>();
		manaBar.target = player.GetComponent<ManaManager>();
        expBar.target = player.GetComponent<Characteristics>();
        statsUI.Characteristics = player.GetComponent<Characteristics>();
        statsUI.Entity = player.GetComponent<Entity>();

		player.GetComponent<Damageable>().onDeath.AddListener(PlayerDie);

		menuInGame.Inventory.onEncumberedChange += PlayerEncumbered;
		menuInGame.Inventory.playerGainQuestItem += PlayerWon;

		uiRootNonPlayer.inventoryDisplayChange += CheckFeedBack;
		menuInGame.onMenuDisplayChange += CheckFeedBack;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ReturnToMainMenu()
	{
		TimeManager.Instance.Pause = false;
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	}

	private void PlayerDie(OnDeathArgs args)
	{
		Camera.main.GetComponent<GameCamera>().enabled = false;
		youDiePanel.gameObject.SetActive(true);
		menuInGame.enabled = false;
	}

	private void PlayerWon()
	{
		TimeManager.Instance.Pause = true;
		menuInGame.enabled = false;
		wonPanel.gameObject.SetActive(true);
	}

	private void PlayerEncumbered()
	{
		if (menuInGame.Inventory.encumbered)
		{
			feedbackText.enabled = true;
			feedbackText.text = "You are over encumbered.";
			displayFeedBackText = true;
		}
		else
		{
			feedbackText.enabled = false;
			feedbackText.text = "";
			displayFeedBackText = false;
		}
	}

	private void CheckFeedBack()
	{
		if (menuInGame.displayed || uiRootNonPlayer.Displayed)
		{
			grabIcon.grabIconUI.SetActive(false);
			grabIcon.enabled = false;
			feedbackText.enabled = false;
		}
		else if (!menuInGame.displayed && !uiRootNonPlayer.Displayed)
		{
			grabIcon.enabled = true;
			if (displayFeedBackText)
				feedbackText.enabled = true;
		}
	}
}
