using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    public int attackDamage = 10;

    private Transform currentTarget; // Mục tiêu hiện tại
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float lastAttackTime;

    [SerializeField] private List<string> staticTargets = new List<string> { "Tower", "Main" }; // Target tĩnh
    [SerializeField] private List<string> dynamicTargets = new List<string> { "Player", "Warrior" }; // Target động
    private float damageResetTime = 5f; // Thời gian quay lại target tĩnh
    private float lastDamageTime;

    [SerializeField] private static Dictionary<string, List<Transform>> allTargets = new Dictionary<string, List<Transform>>();
    private static List<Enemy> allEnemies = new List<Enemy>(); // Thêm danh sách tất cả enemies

    private void Awake()
    {
        foreach (string tag in staticTargets)
        {
            if (!allTargets.ContainsKey(tag))
            {
                allTargets[tag] = new List<Transform>();
            }
        }
        foreach (string tag in dynamicTargets)
        {
            if (!allTargets.ContainsKey(tag))
            {
                allTargets[tag] = new List<Transform>();
            }
        }
    }

    private void OnEnable()
    {
        allEnemies.Add(this); // Thêm enemy vào danh sách khi kích hoạt
        AddAllTargets();
    }

    private void OnDisable()
    {
        allEnemies.Remove(this); // Xóa enemy khỏi danh sách khi hủy
        RemoveFromAllTargets(transform);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        FindStaticTarget();
    }

    private void Update()
    {
        // Quay lại target tĩnh nếu không nhận sát thương trong 5 giây
        if (Time.time > lastDamageTime + damageResetTime && currentTarget != null && dynamicTargets.Contains(currentTarget.tag))
        {
            FindStaticTarget();
        }

        if (currentTarget == null)
        {
            FindStaticTarget(); // Tìm lại mục tiêu tĩnh nếu không có mục tiêu nào
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

    private void AddAllTargets()
    {
        foreach (string tag in staticTargets)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                if (!allTargets[tag].Contains(obj.transform))
                {
                    allTargets[tag].Add(obj.transform);
                }
            }
        }
    }

    public static void RemoveFromAllTargets(Transform target)
    {
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.RemoveTarget(target);
            }
        }
    }
    public void RemoveTarget(Transform target)
    {
        if (currentTarget == target)
        {
            currentTarget = null;
            FindStaticTarget(); // Tìm mục tiêu tĩnh mới
        }
    }

    private void FindStaticTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (string tag in staticTargets)
        {
            foreach (Transform target in allTargets[tag])
            {
                if (target == null) continue;

                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        UpdateTarget(closestTarget);
    }

    public void UpdateTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void TakeDamageFrom(Transform attacker)
    {
        // Cập nhật mục tiêu sang người vừa gây sát thương
        if (dynamicTargets.Contains(attacker.tag))
        {
            UpdateTarget(attacker);
            lastDamageTime = Time.time;
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
                FindStaticTarget();
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
                FindStaticTarget();
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
