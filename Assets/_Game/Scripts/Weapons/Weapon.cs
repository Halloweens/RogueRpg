using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum DamageType
    {
        Physical,
        Nature,
        Fire,
        Lighting,
        Ice
    }

    public DamageType damageType = DamageType.Physical;

    public uint damages = 1u;

    public LayerMask ignoredlayers = 0;

    public virtual void BeginAttack() { }
    public virtual void EndAttack() { }

    public virtual void BeginSecondaryAttack() { }
    public virtual void EndSecondaryAttack() { }
}
