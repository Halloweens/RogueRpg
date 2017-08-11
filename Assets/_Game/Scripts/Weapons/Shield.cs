using System;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon
{
    [Range(0.0f, 1.0f)]
    public float damageReduction = 0.5f;

    private bool blocking = false;

    private Entity wielder = null;
    private Damageable wielderDamageable = null;

    private void Start()
    {
        wielder = GetComponentInParent<Entity>();
        if (wielder == null)
            return;

        wielderDamageable = wielder.GetComponent<Damageable>();
        if (wielderDamageable == null)
            return;

        wielderDamageable.onDamageTaken.AddListener(ApplyDamageReduction);
    }

    public override void BeginAttack()
    {
        blocking = true;
    }

    public override void EndAttack()
    {
        blocking = false;
    }

    private void ApplyDamageReduction(OnDamageTakenArgs args)
    {
        if (blocking)
            args.damageAmount.Value *= (1.0f - damageReduction);
    }
}
