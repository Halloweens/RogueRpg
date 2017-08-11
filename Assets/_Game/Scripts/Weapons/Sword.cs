using System;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public Transform startRaycast = null;
    public Transform endRaycast = null;

    [Range(0.0f, 1.0f)]
    public float critChance;

    public TrailRenderer trailRenderer = null;

    public bool Attacking { set
        {
            attacking = value;
            hitTargets.Clear();
        }
        get { return attacking; }
    }
    private bool attacking = false;

    private List<Entity> hitTargets = new List<Entity>();

    private Entity wielder = null;

    private void Start()
    {
        wielder = GetComponentInParent<Entity>();
        if (trailRenderer)
            trailRenderer.enabled = false;
    }

    public override void BeginAttack()
    {
        Attacking = true;
        if (trailRenderer)
            trailRenderer.enabled = true;
    }

    public override void EndAttack()
    {
        Attacking = false;
        if (trailRenderer)
            trailRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (attacking)
        {
            if (startRaycast == null || endRaycast == null)
            {
                Debug.LogError("Check your parameters in Sword " + gameObject.name);
                return;
            }

            Ray ray = new Ray(startRaycast.position, endRaycast.position - startRaycast.position);
            LayerMask mask = ~ignoredlayers;
            float distance = Vector3.Distance(startRaycast.position, endRaycast.position);

            RaycastHit[] hits = Physics.RaycastAll(ray, distance, mask, QueryTriggerInteraction.Ignore);

            foreach (var hit in hits)
            {
                Entity e = hit.collider.gameObject.GetComponentInParent<Entity>();
                if (e != null && e != wielder && hitTargets.Find(x => e == x) == null)
                {
                    hitTargets.Add(e);

                    Damageable damageable = e.gameObject.GetComponent<Damageable>();
                    if (damageable != null)
                    {
                        float realDamages = damages;
                        bool crit = UnityEngine.Random.Range(0.0f, 1.0f) > critChance;
                        if (crit)
                            realDamages *= 2.0f;

                        damageable.TakeDamage(wielder.gameObject, realDamages, crit);
                    }
                }
            }
        }
    }
}
