using UnityEngine;
using UnityEngine.UI;

public class MenuPlayer : MonoBehaviour
{
	public Text StrText { get { return strText; } }
	[SerializeField] private Text strText;

	public Text ConstText { get { return constText; } }
	[SerializeField] private Text constText;

	public Text IntelText { get { return intelText; } }
	[SerializeField] private Text intelText;

	public Text DextText { get { return dextText; } }
	[SerializeField] private Text dextText;
}
