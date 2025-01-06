using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    public int attackDamage = 10;

    private Transform currentTarget;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float lastAttackTime;

    [SerializeField] private List<string> targetTags = new List<string> { "Tower", "Main", "Player", "Warrior" }; // Danh sách tag cần tấn công
    private float detectionRadius = 100f; // Bán kính phát hiện mục tiêu

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        StartCoroutine(CheckForClosestTarget());
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            FindClosestTarget(); // Tìm lại mục tiêu nếu không có
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("Attack", true);
            animator.SetBool("Run", false);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                AttackCurrentTarget();
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
            animator.SetBool("Attack", false);
            animator.SetBool("Run", true);

            Vector3 direction = agent.velocity;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetTags.Contains(other.tag))
        {
            UpdateTarget(other.transform); // Cập nhật mục tiêu mới khi gặp
        }
    }
    private IEnumerator CheckForClosestTarget()
    {
        while (true)
        {
            FindClosestTarget();
            yield return new WaitForSeconds(1.5f); // Gọi lại sau 1.5 giây
        }
    }

    public void FindClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (string tag in targetTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                if (obj == null) continue;

                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance && distance <= detectionRadius)
                {
                    closestDistance = distance;
                    closestTarget = obj.transform;
                }
            }
        }

        UpdateTarget(closestTarget);
    }

    private void UpdateTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);
        }
    }

    private void AttackCurrentTarget()
    {
        if (currentTarget == null) return;

        if (currentTarget.CompareTag("Player"))
        {
            PlayerHealth playerHealth = currentTarget.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            else
            {
                FindClosestTarget();
            }
        }
        else if (currentTarget.CompareTag("Tower"))
        {
            TowerHealth towerHealth = currentTarget.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(attackDamage);
            }
            else
            {
                FindClosestTarget();
            }
        }
        else if (currentTarget.CompareTag("Main"))
        {
            CastleHealth castleHealth = currentTarget.GetComponent<CastleHealth>();
            if (castleHealth != null)
            {
                castleHealth.TakeDamage(attackDamage);
            }
        }
        else if (currentTarget.CompareTag("Warrior"))
        {
            WarriorHealth warriorHealth = currentTarget.GetComponent<WarriorHealth>();
            if (warriorHealth != null)
            {
                warriorHealth.TakeDamage(attackDamage);
            }
            else
            {
                FindClosestTarget();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.position);
        }
    }
}
