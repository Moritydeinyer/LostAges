using System;
using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    String dire = "South";
    private Animator animator;
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;

    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    private float lastAttackTime;
    public bool active = true;

    public bool hit = false;

    public int attackDamage = 20;
    public float health;
    public float maxHealth = 1f;
    [SerializeField] public Slider healthbar;
    [SerializeField] public TopDownCharacterController characterController;


    private int currentPatrolIndex;
    [SerializeField] public Transform player;

    public Rigidbody2D rb;
    private Vector2 currentVelocity;

    public TilemapCollider2D tilemapCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        try {
            animator = GetComponent<Animator>();
        } catch (Exception e) {}
        currentPatrolIndex = 0;
        health = maxHealth;
        healthbar.value = health;
    }

    IEnumerator attackAnimation()
        {
            String tempDir = dire;
            animator.SetBool("attack"+tempDir, true);
            animator.SetBool("attack", true);
            Debug.Log("Attack"+dire);
            yield return new WaitForSeconds(0.25f);
            animator.SetBool("attack"+tempDir, false);
            animator.SetBool("attack", false);
        }

    IEnumerator hitAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("dmg", false);
        }

    void FixedUpdate()
    {
        if (!active) 
        {
            currentVelocity = Vector2.zero;
            rb.gravityScale = 0; 
            return;
        }
        else
        {
            rb.gravityScale = 1; 
        }
        if (hit) {animator.SetBool("dmg", true); StartCoroutine(hitAnimation()); hit = false;}
        healthbar.value = health;

        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
                currentVelocity = Vector2.zero;
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }
        UpdateAnimation();

        Vector2 newPosition = rb.position + currentVelocity * Time.fixedDeltaTime;

        if (!IsObstacleInPath(newPosition))
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            currentVelocity = Vector2.zero;
        }
        
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool("north", false);
        animator.SetBool("south", false);
        animator.SetBool("east", false);
        animator.SetBool("west", false);
        animator.SetBool("idle", false);

        Vector2 dir = currentVelocity.normalized;

        if (dir == Vector2.zero)
        {
            animator.SetBool("idle", true);
            dire = "South";
            return;
        }

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0) {animator.SetBool("east", true); dire = "East";}
            else {animator.SetBool("west", true); dire = "West";}
        }
        else
        {
            if (dir.y > 0) {animator.SetBool("north", true); dire = "North";}
            else {animator.SetBool("south", true); dire = "South";}
        }
    }



    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        currentVelocity = direction * patrolSpeed;

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            currentVelocity = Vector2.zero; 
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        currentVelocity = direction * patrolSpeed;
    }

    private void AttackPlayer()
    {
        Debug.Log("Attempting to attack player");

    
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("Attacked Player with " + attackDamage + " damage");

            characterController.escMC.gameData.health -= attackDamage;
            characterController.hit = true;
            StartCoroutine(attackAnimation());
        }
    }

    private bool IsObstacleInPath(Vector2 targetPosition)
    {
        if (tilemapCollider != null)
        {
            Vector3 worldPoint = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            if (tilemapCollider.OverlapPoint(worldPoint))
            {
                return true; 
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
