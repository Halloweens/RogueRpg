using UnityEngine;
using UnityEngine.UI;

public class SpellCharacteristics : MonoBehaviour
{
	[SerializeField] private Text nameLabel;
	[SerializeField] private Text description;
	[SerializeField] private Text costLabel;

	[SerializeField] private RawImage image;

	private string costText;

	public MagicInterface MagicInterface { get { return magicInterface; } set { magicInterface = value; } }
	private MagicInterface magicInterface;

	void Awake()
	{
		costText = costLabel.text;

		magicInterface = transform.parent.GetComponent<MagicInterface>();
	}

	public void DisplaySpellCharacteristics(SpellData spell)
	{
		gameObject.SetActive(true);

		nameLabel.text = spell.SpellName.ToUpper();
		description.text = spell.Description;

		costLabel.text = string.Format(costText, spell.Cost);

		image.texture = spell.Icon;
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
