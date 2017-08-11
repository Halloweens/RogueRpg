using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform confirmationNewGamePanel = null;
    [SerializeField]
    private RectTransform confirmationContinuePanel = null;
    [SerializeField]
    private RectTransform statsAssignement = null;

    public Characteristics characteristics = null;
    public Entity entity = null;

    public void OnNewGamePressed()
    {
        confirmationNewGamePanel.gameObject.SetActive(true);
    }

    public void OnNewGameConfirmed()
    {
        confirmationNewGamePanel.gameObject.SetActive(false);
        statsAssignement.gameObject.SetActive(true);
    }

    public void OnNewGameDeclined()
    {
        confirmationNewGamePanel.gameObject.SetActive(false);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void OnContinuePressed()
    {
        confirmationContinuePanel.gameObject.SetActive(true);
        
    }

    public void OnContinueConfirmed()
    {
        confirmationContinuePanel.gameObject.SetActive(false);
        
    }

    public void OnContinueDeclined()
    {
        confirmationContinuePanel.gameObject.SetActive(false);
    }

    public void OnStartGame()
    {
        if (characteristics.RemainingPoints > 0 || entity.entityName == "")
            return;

        BetweenLevelsData bld = SceneManager.GetSceneByName("BetweenLevelsScene").GetRootGameObjects()[0].GetComponent<BetweenLevelsData>();
        
        bld.playerCharacteristics.strength = characteristics.Strength;
        bld.playerCharacteristics.constitution = characteristics.Constitution;
        bld.playerCharacteristics.intelligence = characteristics.Intelligence;
        bld.playerCharacteristics.dexterity = characteristics.Dexterity;
        bld.playerCharacteristics.targetXP = 100;
        bld.playerCharacteristics.remaining = characteristics.RemainingPoints;
        bld.level = 1;
        bld.playerName = entity.entityName;
		bld.hp = 120f + characteristics.Constitution * 1.5f;

		SceneManager.LoadScene("StartZone", LoadSceneMode.Additive);
        SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
    }
}