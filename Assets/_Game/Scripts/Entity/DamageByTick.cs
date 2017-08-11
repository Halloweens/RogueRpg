using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageByTick : MonoBehaviour
{
    public float tickTime = 2.0f;
    public float damageAmount = 10.0f;

    private float timer = 2.0f;

    private List<Damageable> targets = new List<Damageable>();

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            foreach (var t in targets)
                t.TakeDamage(this.gameObject, damageAmount, false);
            timer = tickTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.GetComponentInParent<Entity>();
        if (e != null)
        {
            Damageable d = e.GetComponent<Damageable>();
            if (d != null)
                targets.Add(d);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity e = other.GetComponentInParent<Entity>();
        if (e != null)
        {
            Damageable d = e.GetComponent<Damageable>();
            if (d != null)
                targets.Remove(d);
        }
    }
}
