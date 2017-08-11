using UnityEngine;
using UnityEngine.Events;

public class Usable : MonoBehaviour
{
    public OnUsable onUsable = new OnUsable();

    public bool canUse = true;

    public void Use()
    {
        if (!canUse)
            return;

        if (onUsable != null)
            onUsable.Invoke(new OnUsableArg());
    }

    public void SetUsableState(bool state)
    {
        canUse = state;
    }
}

[System.Serializable]
public class OnUsableArg
{
    public OnUsableArg()
    {
    }
}

[System.Serializable]
public class OnUsable : UnityEvent<OnUsableArg> { }
