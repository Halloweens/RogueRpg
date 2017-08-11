using UnityEngine;
using System.Collections;

public class SpellFireball : Spell
{
    public FireballProjectile fireballProjectile = null;
    public float delay = 0.4f;

    public override void BeginAttack()
    {
        StartCoroutine(BeginBehaviour());
        Destroy(gameObject, 3.0f);
    }

    private IEnumerator BeginBehaviour()
    {
        StartEffects();

        yield return new WaitForSeconds(delay);

        Entity ent = GetComponentInParent<Entity>();

        if (fireballProjectile != null)
        {
            FireballProjectile p = Instantiate(fireballProjectile);
            p.transform.position = transform.position;

            if (ent != null)
            {
                p.ignoredLayers = 1 << ent.gameObject.layer;
                InputSystem inputSystem = ent.gameObject.GetComponent<InputSystem>();
                if (inputSystem != null)
                    p.direction = inputSystem.GetLookDir();
            }
            p.source = ent;
            p.damageAmount = damages;
        }
    }
}
