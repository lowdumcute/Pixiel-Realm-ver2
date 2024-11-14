using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;     // Tầm đánh cận chiến
    public float moveSpeed = 10f;                          // Tốc độ di chuyển
    [SerializeField] private int attackDamage = 5;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isMovingRight = true;                     // Biến để theo dõi hướng di chuyển
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        CheckAttackRange();
    }

    // Hàm xử lý di chuyển
    private void HandleMovement()
    {
        // Lấy đầu vào di chuyển
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Kiểm tra có di chuyển hay không
        bool isMoving = horizontal != 0 || vertical != 0;

        // Nếu không đang tấn công thì cập nhật trạng thái di chuyển
        if (!isAttacking)
        {
            animator.SetBool("Run", isMoving);
        }

        if (isMoving)
        {
            // Di chuyển đối tượng
            transform.position += new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;

            // Lật hình ảnh khi hướng di chuyển thay đổi
            if ((horizontal > 0 && !isMovingRight) || (horizontal < 0 && isMovingRight))
            {
                isMovingRight = !isMovingRight;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
    }

    // Kiểm tra và cập nhật trạng thái tấn công hoặc chạy
    private void CheckAttackRange()
    {
        GameObject enemy = FindNearestEnemy();
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (enemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= attackRange)
            {
                // Có kẻ địch trong tầm đánh, bật Attack và tắt Run
                animator.SetBool("Attack", true);
                animator.SetBool("Run", false);
                isAttacking = true;
            }
            else if (isMoving)
            {
                // Đang di chuyển và không có địch trong tầm đánh, bật Run và tắt Attack
                animator.SetBool("Attack", false);
                animator.SetBool("Run", true);
                isAttacking = false;
            }
            else
            {
                // Không di chuyển và không trong tầm đánh, cả Attack và Run đều tắt
                animator.SetBool("Attack", false);
                animator.SetBool("Run", false);
                isAttacking = false;
            }
        }
        else
        {
            if (isMoving)
            {
                // Không có địch trong tầm đánh, chỉ bật Run khi di chuyển
                animator.SetBool("Attack", false);
                animator.SetBool("Run", true);
                isAttacking = false;
            }
            else
            {
                // Không có địch và không di chuyển, bật trạng thái Idle (tắt cả Attack và Run)
                animator.SetBool("Attack", false);
                animator.SetBool("Run", false);
                isAttacking = false;
            }
        }
    }

    // Tìm Enemy gần nhất
    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    // Hàm thực hiện tấn công lên Enemy, sẽ được gọi từ event trong animation
    public void AttackEnemy()
    {
        GameObject enemy = FindNearestEnemy();
        if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            // Gây sát thương lên enemy nếu trong tầm đánh
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
        isAttacking = false; // Reset lại trạng thái tấn công sau khi tấn công
    }
}
