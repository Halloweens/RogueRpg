using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spell : Weapon
{
    private Animator animator;

    public string triggerName = "Start";

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void StartEffects()
    {
        animator.SetTrigger(triggerName);
    }
}
