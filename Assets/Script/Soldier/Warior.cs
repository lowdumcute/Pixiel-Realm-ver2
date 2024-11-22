using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f; // Tầm đánh cận chiến
    [SerializeField] private float attackCooldown = 1f; // Thời gian hồi chiêu giữa các đòn đánh
    public int attackDamage = 10;
    private Transform currentTarget; // Mục tiêu hiện tại (Enemy)
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float lastAttackTime;
    private static Dictionary<Transform, int> enemyAttackCount = new Dictionary<Transform, int>();

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        FindEnemyTarget();
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            FindEnemyTarget();
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);

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
            animator.SetBool("Run", true);
            animator.SetBool("Attack", false);

            Vector3 direction = agent.velocity;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
    }

    public void FindEnemyTarget()
    {
        // Tìm mục tiêu có tag "Enemy"
        GameObject enemyObject = GameObject.FindWithTag("Enemy");
        if (enemyObject != null)
        {
            currentTarget = enemyObject.transform;
        }
        else
        {
            // Nếu không tìm thấy mục tiêu nào, dừng lại và cập nhật Animator
            currentTarget = null;
            agent.isStopped = true;
            animator.SetBool("Run", false);
            animator.SetBool("Attack", false);
        }
    }

    private void ReleaseEnemyTarget(Transform enemy)
    {
        if (enemyAttackCount.ContainsKey(enemy))
        {
            enemyAttackCount[enemy]--;
            if (enemyAttackCount[enemy] <= 0)
            {
                enemyAttackCount.Remove(enemy);
            }
        }
    }

    private void AttackCurrentTarget()
    {
        EnemyHealth enemyHealth = currentTarget.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(attackDamage);
            if (enemyHealth.currentHealth <= 0)
            {
                ReleaseEnemyTarget(currentTarget);
                FindEnemyTarget();
            }
        }
    }

    public void OnAttackEvent()
    {
        AttackCurrentTarget();
    }

    private void OnDestroy()
    {
        if (currentTarget != null)
        {
            ReleaseEnemyTarget(currentTarget);
        }
    }
}
