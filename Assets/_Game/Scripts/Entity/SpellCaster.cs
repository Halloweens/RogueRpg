using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Arsenal))]
[RequireComponent(typeof(InputSystem))]
[RequireComponent(typeof(CharController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ManaManager))]
public class SpellCaster : MonoBehaviour
{
    private InputSystem inputSystem = null;
    private Arsenal arsenal = null;
    private CharController charController = null;
    private Animator animator = null;
    private ManaManager manaManager = null;

    private Spell spellInstance = null;

    private void Start()
    {
        inputSystem = GetComponent<InputSystem>();
        inputSystem.onSpellCast.AddListener(OnSpellCast);
        inputSystem.onSpellCastUp.AddListener(() => { animator.SetBool("SpellCast", false); });

        arsenal = GetComponent<Arsenal>();
        charController = GetComponent<CharController>();
        animator = GetComponent<Animator>();
        manaManager = GetComponent<ManaManager>();
    }

    private void OnSpellCast(OnSpellCastArgs args)
    {
        if (charController.combatMode)
        {
            SpellData data = arsenal.spells[args.spellIndex];

            if (data != null && manaManager.Mana > data.Cost)
            {
                if (spellInstance)
                    Destroy(spellInstance);

                spellInstance = Instantiate(data.Spell);
                spellInstance.transform.SetParent(arsenal.rightHandSocket, false);

                animator.SetBool("SpellCast", true);
                animator.SetInteger("CastAnimation", (int)data.AnimationType);

                manaManager.Mana -= data.Cost;
            }
        }
    }

    public void BeginSpellPrincipalAttack()
    {
        if (spellInstance != null)
            spellInstance.BeginAttack();
    }

    public void EndSpellPrincipalAttack()
    {
        if (spellInstance != null)
        {
            spellInstance.EndAttack();
            //Destroy(spellInstance.gameObject);
        }
        spellInstance = null;
    }
}
