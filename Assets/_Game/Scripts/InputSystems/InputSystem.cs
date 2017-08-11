using UnityEngine;
using System.Collections;
using UnityEngine.Events;

abstract class InputSystem : MonoBehaviour
{
    public abstract float GetForward();
    public abstract float GetStrafe();
    public abstract Vector3 GetLookDir();
    public abstract bool GetSprint();

    public abstract bool GetJump();
    public abstract bool GetAction();

    public UnityEvent onPrincipalFire = new UnityEvent();
    public UnityEvent onPrincipalFireUp = new UnityEvent();

    public UnityEvent onSecondaryFire = new UnityEvent();
    public UnityEvent onSecondaryFireUp = new UnityEvent();

    public UnityEvent onCombatModeToggle = new UnityEvent();

    public UnityEvent onUse = new UnityEvent();

    public OnSpellCast onSpellCast = new OnSpellCast();
    public UnityEvent onSpellCastUp = new UnityEvent();
}

[System.Serializable]
public class OnSpellCast : UnityEvent<OnSpellCastArgs> { }

[System.Serializable]
public class OnSpellCastArgs
{
    public int spellIndex = 0;

    public OnSpellCastArgs(int index)
    {
        spellIndex = index;
    }
}
