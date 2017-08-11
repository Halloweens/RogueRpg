using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManagerUI : MonoBehaviour {

    public Characteristics target = null;
    
    public Image foreground = null;
    private int curLevel = 1;
    public Text level;

	void Start ()
    {
        target.onStatsChanged.AddListener(OnXpChangeTarget);
        target.onStatsChanged.AddListener(OnLevelUp);
        curLevel = target.Level;
        level.text = curLevel.ToString();
        OnXpChangeTarget();
	}

    private void OnLevelUp()
    {
        curLevel = target.Level;
        level.text = curLevel.ToString();
    }
	private void OnXpChangeTarget()
    {
        if ( target.Experience == 0)
        {
            foreground.transform.localScale = new Vector3(0f, 1.0f, 1.0f);
        }
        else
        {
            foreground.transform.localScale = new Vector3((float)target.Experience / target.TargetExp, 1.0f, 1.0f);
        }
    }
}
