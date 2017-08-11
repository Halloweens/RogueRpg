using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageableUI : MonoBehaviour
{
    public Image foreground = null;

	public Damageable Target { get { return target; } set { target = value; } }
    [SerializeField]
    private Damageable target = null;

    private void Start()
    {
		target.onHealthChange.AddListener(UpdateGraphics);
		foreground.transform.localScale = new Vector3(Mathf.Max((target.hp) / target.maxHP, 0.0f), 1.0f, 1.0f);
	}

    private void UpdateGraphics(OnHealthChangeArgs args)
    {
        foreground.transform.localScale = new Vector3(Mathf.Max((target.hp) / target.maxHP, 0.0f), 1.0f, 1.0f);
    }
}