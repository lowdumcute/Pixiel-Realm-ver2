using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab của đạn
    public Transform firePoint;      // Vị trí bắn ra đạn
    public float fireRate = 1f;      // Tốc độ bắn (giây/viên)
    public float range = 10f;        // Tầm bắn của tháp
    public Vector2 firePointOffset = new Vector2(0.355f, 0.825f); // Offset mặc định của firePoint

    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Animator animator;      // Tham chiếu đến Animator
    private float fireCooldown = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer của Tower
        animator = GetComponent<Animator>(); // Lấy Animator của Tower
    }

    private void Update()
    {
        FindTarget();

        // Nếu không có địch trong tầm bắn
        if (target == null)
        {
            animator.SetBool("Shoot", false); // Dừng animation bắn
            return;
        }

        // Có địch trong tầm bắn
        animator.SetBool("Shoot", true); // Kích hoạt animation bắn

        // Kiểm tra vị trí của mục tiêu để flip tháp
        bool isFlipped = target.position.x < transform.position.x;
        spriteRenderer.flipX = isFlipped;

        // Cập nhật vị trí của firePoint dựa trên flipX
        firePoint.localPosition = isFlipped 
            ? new Vector3(-firePointOffset.x, firePointOffset.y, 0) 
            : new Vector3(firePointOffset.x, firePointOffset.y, 0);

        // Kiểm tra cooldown để bắn
        if (fireCooldown <= 0f)
        {
            fireCooldown = 1f / fireRate; // Đặt lại thời gian cooldown
        }

        fireCooldown -= Time.deltaTime;
    }

    // Tìm địch trong tầm bắn
    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        target = nearestEnemy != null ? nearestEnemy.transform : null;
    }

    // Hàm bắn đạn (gọi từ Animation Event)
    public void Shoot()
    {
        if (target == null) return;

        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
