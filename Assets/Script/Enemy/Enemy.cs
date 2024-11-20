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

    // Danh sách ưu tiên mục tiêu
    [SerializeField] private List<string> targetPriority = new List<string> { "Warior", "Tower", "Main" };

    // Danh sách tĩnh chứa tất cả Enemy
    public static List<Enemy> allEnemies = new List<Enemy>();

    private void Awake()
    {
        // Thêm Enemy vào danh sách
        allEnemies.Add(this);
    }

    private void OnDestroy()
    {
        // Loại bỏ Enemy khỏi danh sách khi bị hủy
        allEnemies.Remove(this);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        FindTarget();
    }

    public void FindTarget()
    {
        Transform newTarget = null;

        foreach (string tag in targetPriority)
        {
            GameObject targetObject = GameObject.FindWithTag(tag);
            if (targetObject != null)
            {
                newTarget = targetObject.transform;
                break; // Ngừng tìm khi đã tìm thấy mục tiêu đầu tiên trong danh sách ưu tiên
            }
        }

        if (newTarget == null)
        {
            Debug.LogWarning("Không tìm thấy mục tiêu nào theo thứ tự ưu tiên.");
        }

        UpdateTarget(newTarget);
    }

    private void UpdateTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            FindTarget(); // Tìm lại mục tiêu nếu bị null
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

    private void AttackCurrentTarget()
    {
        if (currentTarget == null) return;

        if (currentTarget.CompareTag("Tower"))
        {
            TowerHealth towerHealth = currentTarget.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(attackDamage);
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
        else if (currentTarget.CompareTag("Warior"))
        {
            WarriorHealth warriorHealth = currentTarget.GetComponent<WarriorHealth>();
            if (warriorHealth != null)
            {
                warriorHealth.TakeDamage(attackDamage);
            }
        }
    }

    // Phương thức tĩnh để tất cả Enemy tìm mục tiêu mới
    public static void NotifyAllEnemiesToFindTarget()
    {
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.FindTarget();
            }
        }
    }
}
