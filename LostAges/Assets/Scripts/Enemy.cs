using System;
using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Movement & Pathfinding
    public Transform[] patrolPoints;
    private int currentPatrolIndex;

    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    // Health & Damage
    public int attackDamage = 20;
    public float maxHealth = 1f;
    public float health;
    [SerializeField] private Slider healthbar;

    // Story & Character Controller
    [SerializeField] public TopDownCharacterController characterController;

    // Internal State
    public bool hit = false;
    private string dire = "South";
    private Animator animator;

    // Pathfinding
    private Seeker seeker;
    private Rigidbody2D rb;
    private Path path;
    private int currentWaypoint = 0;
    public float nextWaypointDistance = 0.2f;
    public float speed = 5f;

    // Target
    [SerializeField] public Transform player;

    // Attack timing
    private float lastAttackTime;

    // Active flag
    public bool active = true;
    public bool attackable = true;
    public bool isWache = false;





    // AI States
    private enum AIState { Patrolling, Chasing, Attacking }
    private AIState currentState = AIState.Patrolling;
    public float fieldOfViewAngle = 90f;

    // Temporärer Vektor für die Bewegungsrichtung
    private Vector2 currentMovementDirection;
    private Vector2 tempLastPosition;
    private List<Vector2> movementSamples = new List<Vector2>();
    private float sampleTimer = 0f;
    private const float sampleInterval = 0.1f; // Alle 0.2 Sekunden neue Richtung berechnen
    private const int maxSamples = 5;           // Maximal 5 Bewegungen speichern

    [SerializeField] public DynamicCone dynCone; // Referenz auf den DynamicCone-Script

    private float aggroTimeRemaining = 0f;
    private const float aggroDuration = 6f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();

        currentPatrolIndex = 0;
        health = maxHealth;
        if (healthbar) healthbar.value = health;

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();
        currentPatrolIndex = 0;
        health = maxHealth;
        if (healthbar) healthbar.value = health;

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (!active) return;

        Vector2 startPos = rb.position;
        Vector3 targetPos = patrolPoints[currentPatrolIndex].position;

        switch (currentState)
        {
            case AIState.Patrolling:
                targetPos = patrolPoints[currentPatrolIndex].position;
                break;
            case AIState.Chasing:
                if (player != null) targetPos = patrolPoints.Length > 0 ? patrolPoints[currentPatrolIndex].position : startPos;;
                break;
            case AIState.Attacking:
                targetPos = rb.position;
                break;
        }

        if (seeker.IsDone())
        {
            seeker.StartPath(startPos, targetPos, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator attackAnimation()
    {
        string tempDir = dire;
        if (animator)
        {
            animator.SetBool("attack" + tempDir, true);
            animator.SetBool("attack", true);
        }
        yield return new WaitForSeconds(0.25f);
        if (animator)
        {
            animator.SetBool("attack" + tempDir, false);
            animator.SetBool("attack", false);
        }
    }

    IEnumerator hitAnimation()
    {
        if (animator)
            animator.SetBool("dmg", true);
        yield return new WaitForSeconds(0.5f);
        if (animator)
            animator.SetBool("dmg", false);
    }

    private void UpdateAnimation(Vector2 dir)
    {
        if (animator == null) return;

        animator.SetBool("north", false);
        animator.SetBool("south", false);
        animator.SetBool("east", false);
        animator.SetBool("west", false);
        animator.SetBool("idle", false);

        if (dir == Vector2.zero)
        {
            animator.SetBool("idle", true);
            dire = "South";
            return;
        }

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0) { animator.SetBool("east", true); dire = "East"; }
            else { animator.SetBool("west", true); dire = "West"; }
        }
        else
        {
            if (dir.y > 0) { animator.SetBool("north", true); dire = "North"; }
            else { animator.SetBool("south", true); dire = "South"; }
        }
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;
        characterController.escMC.gameData.health -= attackDamage;
        characterController.escMC.audioManager.PlayPlayerHitSFX();
        hit = true;
        StartCoroutine(attackAnimation());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector2 forwardDirection = currentMovementDirection;
        Gizmos.color = Color.green;
        float angle = fieldOfViewAngle / 2f;

        Vector2 startAngle = new Vector2(Mathf.Cos(Mathf.Deg2Rad * -angle), Mathf.Sin(Mathf.Deg2Rad * -angle));
        Vector2 endAngle = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

        startAngle = RotateVector(startAngle, forwardDirection);
        endAngle = RotateVector(endAngle, forwardDirection);

        Gizmos.DrawLine(transform.position, transform.position + (Vector3)startAngle * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)endAngle * detectionRange);

        int segments = 30;
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = Mathf.Lerp(-angle, angle, i / (float)segments);
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * currentAngle), Mathf.Sin(Mathf.Deg2Rad * currentAngle));
            direction = RotateVector(direction, forwardDirection);
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * detectionRange);
        }
    }

    private Vector2 RotateVector(Vector2 vector, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        float rotatedX = cos * vector.x - sin * vector.y;
        float rotatedY = sin * vector.x + cos * vector.y;

        return new Vector2(rotatedX, rotatedY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.activeSelf)
            {
                if (collision.gameObject.name.Contains("gefaengnis TRIGGER") && characterController.escMC.storyManager.checkStoryID("K3_2") && this.name.Contains("playerDummyVerhaftung"))
                {
                    characterController.escMC.storyManager.addStoryID("K3_3");
                }
                if (collision.gameObject.name.Contains("gefaengnis TRIGGER")  && characterController.escMC.storyManager.checkStoryID("K2_2") && this.name.Contains("playerDummyVerhaftung"))
                {
                    characterController.escMC.storyManager.addStoryID("K2_3");
                }
            }
        }


    void FixedUpdate()
    {
        if (isWache && healthbar)
            healthbar.gameObject.SetActive(false);
            
        if (!active)
        {
            rb.velocity = Vector2.zero;
            if (animator != null)
            {
                animator.SetBool("north", false);
                animator.SetBool("south", false);
                animator.SetBool("east", false);
                animator.SetBool("west", false);
                animator.SetBool("idle", true);
            }
            return;
        }

        if (aggroTimeRemaining > 0f)
        aggroTimeRemaining -= Time.fixedDeltaTime;

        if (hit)
        {
            StartCoroutine(hitAnimation());
            hit = false;
        }

        if (healthbar)
            healthbar.value = health;
        

        HandleAI();

        if (path != null && path.vectorPath != null && path.vectorPath.Count > 0)
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                UpdateAnimation(Vector2.zero);
                return;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 move = direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);

            if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            {
                currentWaypoint++;
                if (currentWaypoint >= path.vectorPath.Count && currentState == AIState.Patrolling)
                {
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                }
            }

            UpdateAnimation(direction);
        }
        else
        {
            UpdateAnimation(Vector2.zero);
        }

        // NEU: Bewegungsrichtung immer aktualisieren
        Vector2 moveDeltaFinal = (rb.position - tempLastPosition);
        if (moveDeltaFinal.magnitude > 0.01f)
        {
            movementSamples.Add(moveDeltaFinal.normalized);
            if (movementSamples.Count > maxSamples) movementSamples.RemoveAt(0);
        }

        sampleTimer += Time.fixedDeltaTime;
        if (sampleTimer >= sampleInterval && movementSamples.Count > 0)
        {
            sampleTimer = 0f;

            Vector2 averageDirection = Vector2.zero;
            foreach (var dir in movementSamples)
            {
                averageDirection += dir;
            }
            averageDirection.Normalize();

            currentMovementDirection = Vector2.Lerp(currentMovementDirection, averageDirection, 5f);
        }

        tempLastPosition = rb.position;


        
        if (currentMovementDirection.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(currentMovementDirection.y, currentMovementDirection.x) * Mathf.Rad2Deg;
            if (dynCone != null)
            {
                dynCone.transform.rotation = Quaternion.Euler((90+360-angle), 90f, 90f);
                dynCone.angle = fieldOfViewAngle;
                dynCone.radius = detectionRange;
            }
        }

    }

    private void HandleAI()
    {
        if (player == null) return;

        float distToPlayer = player != null ? Vector2.Distance(transform.position, player.position) : float.MaxValue;

        if (distToPlayer <= detectionRange)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            float angleToPlayer = Vector2.Angle(currentMovementDirection, directionToPlayer);

            if (angleToPlayer <= fieldOfViewAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, LayerMask.GetMask("obstacle"));
                if (!hit)
                {
                    currentState = distToPlayer <= attackRange ? AIState.Attacking : AIState.Chasing;
                    if (distToPlayer <= attackRange && aggroTimeRemaining <= 0f)
                    {
                        aggroTimeRemaining = aggroDuration;
                    }
                }
                else
                {
                    currentState = aggroTimeRemaining > 0 ? AIState.Attacking : AIState.Patrolling;
                }
            }
            else
            {
                currentState = aggroTimeRemaining > 0 ? AIState.Chasing : AIState.Patrolling;
            }
        }
        else
        {
            currentState = AIState.Patrolling;
        }

        switch (currentState)
        {
            case AIState.Patrolling:
                break;
            case AIState.Chasing:
                SetTarget(player.position);
                break;
            case AIState.Attacking:
                TryAttack();
                break;
        }
    }

    private void SetTarget(Vector2 targetPos)
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, targetPos, OnPathComplete);
        }
    }

    private void TryAttack()
    {
        if (player == null) return;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            RaycastHit2D hitInfo = Physics2D.Linecast(transform.position, player.position, LayerMask.GetMask("obstacle"));
            if (!hitInfo)
            {
                if (!isWache)
                {
                    AttackPlayer();
                }
                else if (this.gameObject.name.Contains("WacheTBC"))
                {
                    characterController.escMC.playerTransform.position = characterController.escMC.storyManager.SPTBC1.transform.position;
                }
            }
        }

        currentState = AIState.Patrolling;
    }
}
