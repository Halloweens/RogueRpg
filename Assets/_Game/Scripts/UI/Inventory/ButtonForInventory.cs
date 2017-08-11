using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonForInventory : MonoBehaviour
{
	[SerializeField] private Color selectedColor;

	public Button button;

	private Text text;
	private Color initColor;
	private Color curColor;

	void Awake ()
	{
		button = GetComponent<Button>();

		text = GetComponentInChildren<Text>();
		initColor = text.color;
		curColor = initColor;

		button.onClick.AddListener(delegate { SelectButton(); });
	}
	
	public void ChangeText(string newText)
	{
		text.text = newText;
	}

	public void ChangeTextColor(Color newColor)
	{
		text.color = newColor;
		curColor = newColor;
	}

	public void InitTextColor()
	{
		text.color = initColor;
		curColor = initColor;
	}

	public void SelectButton()
	{
		foreach (Transform child in transform.parent)
			if (child.GetComponent<ButtonForInventory>())
				child.GetComponent<ButtonForInventory>().Unselect();

		text.color = selectedColor;
	}

	public void Unselect()
	{
		text.color = curColor;
	}

	public void DisplayImageEquipped()
	{
		transform.GetChild(1).gameObject.SetActive(true);
	}

	public void HideImageEquipped()
	{
		transform.GetChild(1).gameObject.SetActive(false);
	}

	public void DisplayUpgradeArrow()
	{
		transform.GetChild(2).gameObject.SetActive(true);
	}

	public void HideUpgradeArrow()
	{
		transform.GetChild(2).gameObject.SetActive(false);
	}

	public bool ArrowDisplayed()
	{
		return transform.GetChild(2).gameObject.activeSelf;
	}
}

