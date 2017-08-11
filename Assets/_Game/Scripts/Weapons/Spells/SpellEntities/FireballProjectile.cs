using UnityEngine;
using System.Collections;

public class FireballProjectile : MonoBehaviour
{
    public LayerMask ignoredLayers = 0;
    public float speed = 25.0f;
    public float damageAmount = 10.0f;
    public Entity source = null;

    public Vector3 direction = Vector3.forward;

    private void Start()
    {
        Destroy(gameObject, 30.0f);
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ignoredLayers == (ignoredLayers | (1 << other.gameObject.layer)))
            return;

        if (other.isTrigger)
            return;

        Entity entity = other.GetComponentInParent<Entity>();
        if (entity != null)
        {
            Damageable damageable = entity.gameObject.GetComponent<Damageable>();
            if (damageable != null && damageable.Hp <= 0)
                return;
            if (damageable != null)
                damageable.TakeDamage(source != null ? source.gameObject : null, damageAmount, false);
        }

        Destroy(gameObject);
    }
}
