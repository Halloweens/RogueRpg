using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuRoot : MonoBehaviour
{
	void Awake()
	{
		SceneManager.LoadScene("BetweenLevelsScene", LoadSceneMode.Additive);
	}
}
