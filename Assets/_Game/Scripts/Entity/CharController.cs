using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputSystem))]
public class CharController : MonoBehaviour
{
    public float baseAnimationRatio = 1.0f;

    public float directionalMovementBlendInterpolationFactor = 5.0f;
    public float rotationMovementBlendInterpolationFactor = 10.0f;
    public bool autoRegisterToInputSystem = true;
    public bool combatMode = false;

	public bool CanMove { get { return canMove; } set { canMove = value; } }
	private bool canMove = true;

    private Animator animator = null;
    private InputSystem inputSystem = null;

    private float forwardInput = 0.0f;
    private float strafeInput = 0.0f;
    private bool principalFireInput = false;
    private bool secondaryFireInput = false;
    private bool jumpInput = false;
    private bool sprintInput = false;

    private float forwardBlend = 0.0f;
    private float strafeBlend = 0.0f;

    public bool IsTurnedAround { get { return isTrunedAround; } }
    private bool isTrunedAround = false;

    private Characteristics characteristics = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputSystem = GetComponent<InputSystem>();

        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

        if (autoRegisterToInputSystem)
        {
            inputSystem.onPrincipalFire.AddListener(new UnityAction(() => { principalFireInput = true; }));
            inputSystem.onPrincipalFireUp.AddListener(new UnityAction(() => { principalFireInput = false; }));
            inputSystem.onSecondaryFire.AddListener(new UnityAction(() => { secondaryFireInput = true; }));
            inputSystem.onSecondaryFireUp.AddListener(new UnityAction(() => { secondaryFireInput = false; }));
            inputSystem.onCombatModeToggle.AddListener(new UnityAction(() => { combatMode = !combatMode; }));
        }

        characteristics = GetComponent<Characteristics>();
        if (characteristics != null)
            characteristics.onStatsChanged.AddListener(RecomputeAnimationRatio);

        RecomputeAnimationRatio();
    }

    private void Update()
    {
        forwardInput = inputSystem.GetForward();
        strafeInput = inputSystem.GetStrafe();
        sprintInput = inputSystem.GetSprint();
        jumpInput = inputSystem.GetJump();
    }

    private void FixedUpdate()
    {
        Vector3 groundProjectedDir = Vector3.ProjectOnPlane(inputSystem.GetLookDir(), Vector3.up);

        float angle = Vector3.Angle(Vector3.forward, groundProjectedDir);

        if (Vector3.Dot(groundProjectedDir, Vector3.right) < 0.0f)
            angle = -angle;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, angle, 0.0f));

        float multiplier = sprintInput ? 2.0f : 1.0f;

        float finalForwardInput = forwardInput;
        float finalStrafeInput = strafeInput;

        if (!combatMode)
        {
            if (forwardInput < 0.0f)
                isTrunedAround = true;
            else if (forwardInput > 0.0f)
                isTrunedAround = false;
        }
        else
        {
            isTrunedAround = false;

            if (secondaryFireInput)
                multiplier = 1.0f;
        }

        if (isTrunedAround)
        {
            finalForwardInput = Mathf.Abs(forwardInput);
            finalStrafeInput = -strafeInput;
            targetRotation = targetRotation * Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

		forwardBlend = canMove ? Mathf.Lerp(forwardBlend, finalForwardInput * multiplier, Time.fixedDeltaTime * directionalMovementBlendInterpolationFactor) : 0f;
		strafeBlend = canMove ? Mathf.Lerp(strafeBlend, finalStrafeInput * multiplier, Time.fixedDeltaTime * directionalMovementBlendInterpolationFactor) : 0f;

        animator.SetFloat("SpeedForward", forwardBlend);
        animator.SetFloat("SpeedStrafe", strafeBlend);
        animator.SetBool("CombatMode", combatMode);
        animator.SetBool("PrincipalFire", principalFireInput);
        animator.SetBool("AlternateFire", secondaryFireInput);
        animator.SetBool("Jump", jumpInput);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationMovementBlendInterpolationFactor);
    }

    private void RecomputeAnimationRatio()
    {
        float animationRatio = baseAnimationRatio;

        if (characteristics != null)
            animationRatio += characteristics.Dexterity * 0.01f;

        animationRatio = Mathf.Min(animationRatio, 1.5f);

        animator.speed = animationRatio;
    }
}
