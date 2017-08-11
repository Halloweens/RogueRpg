using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputSystem))]
public class IKAimController : MonoBehaviour
{
    public bool ikActive = true;

    public float weight = 1.0f;
    public float bodyWeight = 0.8f;
    public float headWeight = 1.0f;

    private Animator animator = null;
    private InputSystem inputSystem = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputSystem = GetComponent<InputSystem>();
    }

    private void OnAnimatorIK()
    {
        if (ikActive)
        {
            Vector3 dir = inputSystem.GetLookDir();

            animator.SetLookAtWeight(weight, bodyWeight, headWeight);
            animator.SetLookAtPosition(transform.position + dir * 5.0f);
        }
        else
        {
            animator.SetLookAtWeight(0.0f);
        }
    }
}