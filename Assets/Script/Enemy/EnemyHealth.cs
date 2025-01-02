using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    private Flash flash;
    private bool isDead = false;


    // Tham chiếu tới EnemyHealthBar để cập nhật thanh máu
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private Enemy enemy;
    public KillEnemiesProgress killEnemiesProgress;
    [SerializeField] private GameObject deathEffectPrefab;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        // Nếu chưa có tham chiếu tới healthBar, tìm kiếm nó trong con
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<EnemyHealthBar>();
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);  // Cập nhật thanh máu ngay từ đầu
        }
         {
        // Lấy tham chiếu đến KillEnemiesProgress nếu chưa có
        if (killEnemiesProgress == null)
        {
            killEnemiesProgress = FindObjectOfType<KillEnemiesProgress>();
        }
    }
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        if (isDead) return;
        Enemy enemyScript = GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.FindClosestTarget();
            }
        currentHealth -= Mathf.Max(damage - GetDefense(), 0); // Đảm bảo máu không dưới 0
        StartCoroutine(flash.FlashRountine());

        // Cập nhật thanh máu
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        

        // Kiểm tra nếu máu <= 0 thì chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private int GetDefense()
    {
        return 0;
    }

    private void Die()
    {
        if (isDead) return; // Tránh gọi Die nhiều lần
        isDead = true;

        // Gửi sự kiện lên SpawnManager và KillEnemiesProgress
        FindObjectOfType<SpawnManager>().OnEnemyDefeated();
        if (killEnemiesProgress != null)
        {
            killEnemiesProgress.OnEnemyKilled();
        }

        // Spawn hiệu ứng chết
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Hủy đối tượng
        Destroy(gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
