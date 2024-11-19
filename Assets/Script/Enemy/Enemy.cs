using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f; // Tầm đánh cận chiến
    [SerializeField] private float attackCooldown = 1f; // Thời gian hồi chiêu
    [SerializeField] private float detectionRadius = 5f; // Bán kính phát hiện mục tiêu
    public int attackDamage = 10;

    private Transform mainTarget; // Mục tiêu chính
    private Transform currentTarget; // Mục tiêu hiện tại
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float lastAttackTime;

    // Static list giữ tất cả Enemy
    public static List<Enemy> allEnemies = new List<Enemy>();

    private void Start()
    {
        // Thêm vào danh sách tất cả Enemy
        allEnemies.Add(this);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        FindTarget();
    }

    private void OnDestroy()
    {
        // Xóa khỏi danh sách khi Enemy bị hủy
        allEnemies.Remove(this);
    }

    public void FindTarget()
    {
        // Tìm "Warrior" nếu tồn tại
        GameObject warriorObject = GameObject.FindWithTag("Warior");
        if (warriorObject != null)
        {
            mainTarget = warriorObject.transform;
        }

        // Tìm "Tower" nếu tồn tại
        GameObject towerObject = GameObject.FindWithTag("Tower");
        if (towerObject != null)
        {
            mainTarget = towerObject.transform;
        }

        // Tìm "Main" nếu không tìm thấy Tower
        if (mainTarget == null)
        {
            GameObject mainObject = GameObject.FindWithTag("Main");
            if (mainObject != null)
            {
                mainTarget = mainObject.transform;
            }
        }

        if (mainTarget == null)
        {
            Debug.LogWarning("Không tìm thấy mục tiêu nào (Warrior, Tower, hoặc Main).");
            return;
        }

        currentTarget = mainTarget; // Đặt mục tiêu ban đầu
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            FindTarget(); // Tìm lại mục tiêu nếu bị null
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        // Nếu trong tầm đánh
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
            // Di chuyển về phía mục tiêu
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
            animator.SetBool("Attack", false);
            animator.SetBool("Run", true);

            // Lật hướng Enemy theo hướng di chuyển
            Vector3 direction = agent.velocity;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
    }

    private void AttackCurrentTarget()
    {
        if (currentTarget.CompareTag("Tower"))
        {
            TowerHealth towerHealth = currentTarget.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(attackDamage);

                // Tìm mục tiêu mới nếu Tower bị phá hủy
                if (towerHealth.currentHealth <= 0)
                {
                    Debug.Log("Tower đã bị phá hủy, tìm mục tiêu mới...");
                    FindTarget();
                }
            }
        }
        else if (currentTarget.CompareTag("Main"))
        {
            CastleHealth castleHealth = currentTarget.GetComponent<CastleHealth>();
            if (castleHealth != null)
            {
                castleHealth.TakeDamage(attackDamage);
            }
        else if (currentTarget.CompareTag("Warior"))
        {
            WariorHealth wariorHealth  = currentTarget.GetComponent<WariorHealth>();
            if (wariorHealth != null)
            {
                wariorHealth.TakeDamage(attackDamage);
                if (wariorHealth.currentHealth <= 0)
                {
                    FindTarget();
                }
            }
        }
        }
    }

    // Gây sát thương từ Animation Event
    public void OnAttackEvent()
    {
        AttackCurrentTarget();
    }
}
