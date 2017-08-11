using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
    public float Xp
    {
        get { return xp; }
        set
        {
            xp = value;
            if (onXpChanged != null)
                onXpChanged.Invoke(new OnXpChangedArgs(xp));
        }
    }

    private float xp;

    public Player Target { get { return target; } set { target = value; } }
    private Player target = null;
     
    void Start()
    {
        target = GetComponent<Player>();
        xp = target.Characteristics.Experience;
    }

    public OnXpChanged onXpChanged = new OnXpChanged();

}
[System.Serializable]
public class OnXpChanged : UnityEvent<OnXpChangedArgs> { }

[System.Serializable]
public class OnXpChangedArgs
{
    public float newXpValue = 0.0f;

    public OnXpChangedArgs(float v)
    {
        newXpValue = v;
    }
}

