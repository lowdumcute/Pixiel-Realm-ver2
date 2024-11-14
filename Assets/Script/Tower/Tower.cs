using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab của đạn
    public Transform firePoint;      // Vị trí bắn ra đạn
    public float fireRate = 1f;      // Tốc độ bắn (giây/viên)
    public float range = 10f;        // Tầm bắn của tháp

    private Transform target;
    private float fireCountdown = 0f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer của Tower
    }

    private void Update()
    {
        FindTarget();
        
        if (target == null)
            return;

        // Kiểm tra vị trí của mục tiêu để flip tháp
        if (target.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
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

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Hàm bắn đạn
    private void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
