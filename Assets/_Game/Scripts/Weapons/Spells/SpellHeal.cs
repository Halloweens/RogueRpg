using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHeal : Spell
{
    public override void BeginAttack()
    {
        StartCoroutine(BeginBehaviour());
        Destroy(gameObject, 3.0f);
    }

    private IEnumerator BeginBehaviour()
    {
        StartEffects();

        yield return new WaitForSeconds(1.0f);

        Entity target = GetComponentInParent<Entity>();
        if (target != null)
        {
            Damageable damageable = target.gameObject.GetComponent<Damageable>();
            if (damageable != null)
                damageable.Hp += damages;
        }
    }
}
