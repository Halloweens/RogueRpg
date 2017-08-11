using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class ChangeBehaviourStateOnDeath : MonoBehaviour
{
    public bool setAnimatorTrigger = true;
    public string deathTriggerName = "Death";

    public Item[] items = new Item[0];

    private Damageable damageable = null;

    private void Start()
    {
        damageable = GetComponent<Damageable>();

        damageable.onDeath.AddListener(OnDeath);
    }

    private void OnDeath(OnDeathArgs args)
    {
        foreach (var i in items)
        {
            if (i.obj == null)
                return;

            if (i.obj is MonoBehaviour)
            {
                MonoBehaviour b = i.obj as MonoBehaviour;
                b.enabled = i.state;
            }
            else if (i.obj is Collider)
            {
                Collider c = i.obj as Collider;
                c.enabled = i.state;
            }
            else if (i.obj is GameObject)
            {
                GameObject go = i.obj as GameObject;
                go.SetActive(i.state);
            }
            else
                Debug.LogError(i.obj.name + " can't be handled by this script.");
        }

        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger(deathTriggerName);
    }

    [System.Serializable]
    public struct Item
    {
        public UnityEngine.Object obj;
        public bool state;
    }
}

