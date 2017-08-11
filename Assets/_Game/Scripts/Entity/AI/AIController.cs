using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIInputSystem))]
[RequireComponent(typeof(CharController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Pathfinding))]

public class AIController : MonoBehaviour
{
    public Sensor sensor = null;
    public bool aggroOnAggro = false;
    public float helpCryRadius = 10.0f;
    public float attackRangeEpsilon = 0.5f;
    public float range = 2.0f;
    public float slowRange = 3.0f;

    public float attackRate = 1.5f;
    public float randomWaitTime = 2.0f;
    public bool canUseCombo = true;
    [Range(0.0f, 100.0f)]
    public float comboRate = 50.0f;

    public bool showDebug = false;

    private Vector3 lastKnownTargetPosition = Vector3.zero;
    private Entity target = null;
    private Coroutine attackBehaviour = null;

    private AIInputSystem inputSystem = null;
    private CharController charController = null;
    private Animator animator = null;

    private Pathfinding pathfinding;

    private void Start()
    {
        if (sensor == null)
        {
            Debug.LogError("Ai " + gameObject.name + " doesn't have a sensor !");
            return;
        }

        sensor.onTagDetected.AddListener(OnEnemyDetected);

        inputSystem = GetComponent<AIInputSystem>();
        charController = GetComponent<CharController>();
        animator = GetComponent<Animator>();
        pathfinding = GetComponent<Pathfinding>();
    }

    private void FixedUpdate()
    {
        inputSystem.SetLookDir(transform.forward);

        if (lastKnownTargetPosition != Vector3.zero)
        {
            float speed = 2.0f;
            float dist = Vector3.Distance(transform.position, lastKnownTargetPosition);
            float entityDist = target != null ? Vector3.Distance(transform.position, target.transform.position) : float.MaxValue;

            if (dist < slowRange)
                speed = 2.0f * ((dist - range) / (slowRange - range));

            if (entityDist > range - attackRangeEpsilon && entityDist < range + attackRangeEpsilon)
            {
                if (attackBehaviour == null)
                    attackBehaviour = StartCoroutine(AttackBehaviour());
            }
            else if (attackBehaviour != null)
            {
                StopCoroutine(attackBehaviour);
                attackBehaviour = null;
                inputSystem.onPrincipalFireUp.Invoke();
            }

            if (sensor.IsPlayerInSight())
            {
                inputSystem.SetLookDir(target.transform.position - transform.position);
                pathfinding.Path.Clear();
            }
            else if (pathfinding != null)
            {
                if (pathfinding.Path.Count <= 0)
                    pathfinding.FindPath(transform.position, lastKnownTargetPosition);
                else
                {
                    inputSystem.SetLookDir(Vector3.Normalize(pathfinding.Path[0] - transform.position));
                    pathfinding.Path.Clear();
                }
            }
            inputSystem.SetForward(speed);
        }

    }

    private void OnEnemyDetected(OnSensorTaggedEntityDetectedArgs args)
    {
        target = args.entity;
        lastKnownTargetPosition = args.entity.transform.position;

        if (charController.combatMode != true)
        {
            animator.SetTrigger("Taunt");
            inputSystem.onCombatModeToggle.Invoke();

            CryForHelp();
        }
    }

    private void CryForHelp()
    {
        LayerMask mask = 0;
        mask |= 1 << gameObject.layer;

        Collider[] colliders = Physics.OverlapSphere(transform.position, helpCryRadius, mask, QueryTriggerInteraction.Ignore);
        foreach (var c in colliders)
        {
            Entity e = c.GetComponentInParent<Entity>();
            if (e != null)
            {
                AIController aiController = e.gameObject.GetComponent<AIController>();
                if (aiController != null)
                {
                    if (aiController.charController.combatMode != true)
                        aiController.inputSystem.onCombatModeToggle.Invoke();
                    aiController.lastKnownTargetPosition = lastKnownTargetPosition;

                    if (aiController.aggroOnAggro)
                        aiController.CryForHelp();
                }
            }
        }
    }

    private IEnumerator AttackBehaviour()
    {
        while (true)
        {
            if (target != null)
            {
                Damageable damageable = target.GetComponent<Damageable>();
                if (damageable != null && damageable.Dead)
                    animator.SetTrigger("Taunt");
                else
                    inputSystem.onPrincipalFire.Invoke();
            }
            else
                inputSystem.onPrincipalFire.Invoke();

            if (canUseCombo)
            {
                if (Random.Range(0.0f, 100.0f) <= comboRate)
                    yield return new WaitForSeconds(2.0f);
            }

            yield return new WaitForSeconds(0.1f);

            inputSystem.onPrincipalFireUp.Invoke();

            yield return new WaitForSeconds(attackRate);
            yield return new WaitForSeconds(Random.Range(0.0f, randomWaitTime));
        }
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, helpCryRadius);
        }
    }
}
