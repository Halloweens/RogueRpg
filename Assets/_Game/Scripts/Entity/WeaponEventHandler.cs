using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharController))]
[RequireComponent(typeof(Arsenal))]
public class WeaponEventHandler : MonoBehaviour
{
    public Transform weaponSheat = null;

    private Arsenal arsenal = null;
    private CharController charController = null;

    private void Awake()
    {
        arsenal = GetComponent<Arsenal>();
        arsenal.onWeaponsChanged.AddListener(InitWeaponPos);

        charController = GetComponent<CharController>();
    }

    public void BeginPrincipalAttack(WeaponHandSocket side)
    {
        Weapon winstance = side == WeaponHandSocket.Left ? arsenal.LeftHandWeaponInstance : arsenal.RightHandWeaponInstance;
        if (winstance != null)
            winstance.BeginAttack();
    }

    public void EndPrincipalAttack(WeaponHandSocket side)
    {
        Weapon winstance = side == WeaponHandSocket.Left ? arsenal.LeftHandWeaponInstance : arsenal.RightHandWeaponInstance;

        if (winstance != null)
            winstance.EndAttack();
    }

    public void BeginSecondaryAttack(WeaponHandSocket side)
    {
        Weapon winstance = side == WeaponHandSocket.Left ? arsenal.LeftHandWeaponInstance : arsenal.RightHandWeaponInstance;
        if (winstance != null)
            winstance.BeginSecondaryAttack();
    }

    public void EndSecondaryAttack(WeaponHandSocket side)
    {
        Weapon winstance = side == WeaponHandSocket.Left ? arsenal.LeftHandWeaponInstance : arsenal.RightHandWeaponInstance;
        if (winstance != null)
            winstance.EndSecondaryAttack();
    }

    public void SheatWeapon(WeaponHandSocket side)
    {
        Weapon winstance;
        if (side == WeaponHandSocket.Right)
        {
            winstance = arsenal.RightHandWeaponInstance;
            if (winstance != null && weaponSheat != null)
                winstance.transform.SetParent(weaponSheat, false);
        }
        else
        {
            winstance = arsenal.LeftHandWeaponInstance;
            if (winstance != null && weaponSheat != null)
                winstance.transform.SetParent(weaponSheat, false);
        }
    }

    public void DrawWeapon(WeaponHandSocket side)
    {
        Weapon winstance;
        if (side == WeaponHandSocket.Right)
        {
            winstance = arsenal.RightHandWeaponInstance;
            if (winstance != null && arsenal.rightHandSocket != null)
                winstance.transform.SetParent(arsenal.rightHandSocket, false);
        }
        else
        {
            winstance = arsenal.LeftHandWeaponInstance;
            if (winstance != null && arsenal.leftHandSocket != null)
                winstance.transform.SetParent(arsenal.leftHandSocket, false);
        }
    }

    public void InitWeaponPos()
    {
        if (!charController.combatMode)
        {
            SheatWeapon(WeaponHandSocket.Right);
            SheatWeapon(WeaponHandSocket.Left);
        }
    }
}
