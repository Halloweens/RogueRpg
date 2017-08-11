using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Sensor : MonoBehaviour
{
    public OnSensorTaggedEntityDetected onTagDetected = new OnSensorTaggedEntityDetected();
    public string lookupTag = "Player";
    public bool ignoreSelfLayer = true;
    public bool showDebug = false;

    private new Collider collider = null;
    private bool isSeeingPlayer = false;
    private List<Entity> watchedEntities = new List<Entity>();

    private void Start()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void FixedUpdate()
    {
        foreach (var e in watchedEntities)
        {
            Ray ray = new Ray(transform.position, e.transform.position - transform.position);
            float dist = Vector3.Distance(transform.position, e.transform.position);
            LayerMask mask = 0;
            if (ignoreSelfLayer)
            {
                mask |= 1 << gameObject.layer;
                mask = ~mask;
            }
            else
                mask = int.MaxValue;

            if (showDebug)
                Debug.DrawLine(transform.position, e.transform.position);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, dist, mask, QueryTriggerInteraction.Ignore))
            {
                Entity otherEntity = hit.collider.GetComponentInParent<Entity>();
                
                if (otherEntity != null && otherEntity == e)
                {
                    if (onTagDetected != null)
                    {
                        onTagDetected.Invoke(new OnSensorTaggedEntityDetectedArgs(e, lookupTag));
                        isSeeingPlayer = true;
                        continue;
                    }
                }
            }
            
            isSeeingPlayer = false;

        }
    }

    public bool IsPlayerInSight()
    {
        return isSeeingPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.GetComponentInParent<Entity>();
        if (e != null && e.gameObject.tag == lookupTag)
            watchedEntities.Add(e);
    }

    private void OnTriggerExit(Collider other)
    {
        Entity e = other.GetComponentInParent<Entity>();
        if (watchedEntities.Contains(e))
            watchedEntities.Remove(e);
    }
}

[System.Serializable]
public class OnSensorTaggedEntityDetected : UnityEvent<OnSensorTaggedEntityDetectedArgs> {}

[System.Serializable]
public class OnSensorTaggedEntityDetectedArgs
{
    public Entity entity = null;
    public string lookupTag = "";

    public OnSensorTaggedEntityDetectedArgs(Entity e, string tag)
    {
        entity = e;
        lookupTag = tag;
    }
}
