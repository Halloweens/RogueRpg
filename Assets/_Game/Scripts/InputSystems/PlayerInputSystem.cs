using UnityEngine;
using System.Collections;
using System;

class PlayerInputSystem : InputSystem
{
    public Camera playerCamera = null;

    public override float GetForward()
    {
        return Input.GetAxis("Vertical");
    }

    public override float GetStrafe()
    {
        return Input.GetAxis("Horizontal");
    }

    public override Vector3 GetLookDir()
    {
        if (playerCamera)
            return playerCamera.transform.forward;

        return Vector3.forward;
    }

    public override bool GetSprint()
    {
        return Input.GetButton("Sprint");
    }

    public override bool GetJump()
    {
        return Input.GetButton("Jump");
    }

    public override bool GetAction()
    {
        return Input.GetButton("Action");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && onPrincipalFire != null)
            onPrincipalFire.Invoke();

        if (Input.GetButtonDown("Fire2") && onSecondaryFire != null)
            onSecondaryFire.Invoke();

        if (Input.GetButtonUp("Fire1") && onPrincipalFireUp != null)
            onPrincipalFireUp.Invoke();

        if (Input.GetButtonUp("Fire2") && onSecondaryFireUp != null)
            onSecondaryFireUp.Invoke();

        if (Input.GetButtonDown("ToggleCombatMode") && onCombatModeToggle != null)
            onCombatModeToggle.Invoke();

        if (Input.GetButtonDown("Action") && onUse != null)
            onUse.Invoke();


        //Spells
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetButtonDown("Spell" + i) && onSpellCast != null)
                onSpellCast.Invoke(new OnSpellCastArgs(i));

            if (Input.GetButtonUp("Spell" + i) && onSpellCastUp != null)
                onSpellCastUp.Invoke();
        }

    }
}
