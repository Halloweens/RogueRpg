using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManaManagerUI : MonoBehaviour
{
    public ManaManager target = null;

    public Image foreground = null;
    
    private void Start()
    {
        target.onManaChanged.AddListener(OnManaTargetChanged);
		foreground.transform.localScale = new Vector3(Mathf.Max((target.Mana) / target.MaxMana, 0.0f), 1.0f, 1.0f);
	}

    private void OnManaTargetChanged(OnManaChangedArgs args)
    {
        foreground.transform.localScale = new Vector3(Mathf.Max((target.Mana) / target.MaxMana, 0.0f), 1.0f, 1.0f);
    }
}
