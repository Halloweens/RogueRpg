using UnityEngine;
using System.Collections;
using System;

class AIInputSystem : InputSystem
{
    private float forwardValue = 0.0f;
    private float strafeValue = 0.0f;
    private Vector3 lookDir = Vector3.zero;

    public void SetForward(float value)
    {
        forwardValue = value;
    }

    public void SetStrafe(float value)
    {
        strafeValue = value;
    }

    public void SetLookDir(Vector3 value)
    {
        lookDir = value;
    }

    public override float GetForward()
    {
        return forwardValue;
    }

    public override float GetStrafe()
    {
        return strafeValue;
    }

    public override Vector3 GetLookDir()
    {
        return lookDir;
    }

    public override bool GetSprint()
    {
        return false;
    }

    public override bool GetJump()
    {
        return false;
    }

    public override bool GetAction()
    {
        return false;
    }
}
