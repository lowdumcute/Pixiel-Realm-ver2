using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f; // Tầm đánh cận chiến
    [SerializeField] private float attackCooldown = 1f; // Thời gian hồi chiêu giữa các đòn đánh
    [SerializeField] private float detectionRadius = 5f; // Bán kính phát hiện "Tower"
    public int attackDamage = 10;
    private Transform mainTarget; // Mục tiêu chính (Main)
    private Transform currentTarget; // Mục tiêu hiện tại (Tower hoặc Main)
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float lastAttackTime;

    // Static list để giữ tất cả enemy cùng tham chiếu đến
    public static List<Enemy> allEnemies = new List<Enemy>();

    private void Start()
    {
        // Thêm bản sao của enemy vào danh sách tất cả enemy
        allEnemies.Add(this);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        findTarget();
    }

    private void OnDestroy()
    {
        // Xóa khỏi danh sách khi enemy bị hủy
        allEnemies.Remove(this);
    }

    private void findTarget()
    {
        // Tìm mục tiêu có tag "Tower"
        GameObject towerObject = GameObject.FindWithTag("Tower");
        if (towerObject != null)
        {
            mainTarget = towerObject.transform;
            currentTarget = mainTarget; // Đặt mục tiêu ban đầu là Tower
        }
        else
        {
            // Nếu không tìm thấy Tower, tìm đối tượng có tag "Main"
            GameObject mainObject = GameObject.FindWithTag("Main");
            if (mainObject != null)
            {
                mainTarget = mainObject.transform;
                currentTarget = mainTarget; // Đặt mục tiêu là Main
            }
            else
            {
                Debug.LogWarning("Không tìm thấy đối tượng có tag 'Tower' hoặc 'Main'.");
            }
        }

        // Tìm lại mục tiêu cho tất cả các enemy khác
        foreach (Enemy enemy in allEnemies)
        {
            enemy.SetTarget(mainTarget);  // Đặt lại mục tiêu cho tất cả enemy
        }
    }

    // Đặt lại mục tiêu cho enemy
    private void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            findTarget(); // Tìm lại Tower nếu currentTarget là null
            return;
        }

        // Di chuyển đến mục tiêu hiện tại và kiểm tra khoảng cách
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        // Nếu trong tầm đánh, kích hoạt animation attack và đặt bool Attack = true
        if (distanceToTarget <= attackRange)
        {
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
            // Nếu ra ngoài tầm đánh, ngừng attack và chạy
            animator.SetBool("Attack", false);
            animator.SetBool("Run", true);

            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);

            // Cập nhật hướng di chuyển và thay đổi flipX chỉ khi di chuyển
            Vector3 direction = agent.velocity;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
    }

    // Tấn công mục tiêu hiện tại
    private void AttackCurrentTarget()
    {
        if (currentTarget.CompareTag("Tower"))
        {
            TowerHealth towerHealth = currentTarget.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(attackDamage); // Gây damage lên tower

                // Kiểm tra nếu Tower đã hết máu, tìm Tower khác cho tất cả các enemy
                if (towerHealth.currentHealth <= 0)
                {
                    Debug.Log("Tower is destroyed, finding new target...");
                    findTarget(); // Tìm lại Tower mới nếu Tower hiện tại đã chết
                }
            }
        }
        else if (currentTarget.CompareTag("Main"))
        {
            CastleHealth castleHealth = currentTarget.GetComponent<CastleHealth>();
            if (castleHealth != null)
            {
                castleHealth.TakeDamage(attackDamage); // Gây damage lên Castle
            }
        }
    }

    // Method này sẽ được gọi từ Animation Event để gây sát thương khi animation đến vị trí yêu cầu
    public void OnAttackEvent()
    {
        AttackCurrentTarget(); // Gây damage vào lúc sự kiện được kích hoạt
    }
}
