using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    private Flash flash;
    [SerializeField] private GameObject damageVFXPrefab; // Prefab hiệu ứng trúng đòn
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
            healthBar.UpdateHealthBar(currentHealth, maxHealth); // Cập nhật thanh máu ngay từ đầu
        }

        if (killEnemiesProgress == null)
        {
            killEnemiesProgress = FindObjectOfType<KillEnemiesProgress>();
        }
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        if (isDead) return;

        // Gọi phương thức FindClosestTarget() nếu là Enemy
        Enemy enemyScript = GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.FindClosestTarget();
        }
        AudioManager.instance.PlaySFX("Hit");
        // Giảm máu với sát thương trừ đi phòng thủ
        currentHealth -= Mathf.Max(damage - GetDefense(), 0);

        // Hiển thị hiệu ứng trúng đòn (Flash)
        StartCoroutine(flash.FlashRountine());

        // Spawn hiệu ứng trúng đòn
        SpawnDamageVFX(attacker);

        // Cập nhật thanh máu
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Kiểm tra nếu máu <= 0 thì chết
        if (currentHealth <= 0)
        {
            SpawnDamageVFX(attacker);
            Die();
        }
    }

    private void SpawnDamageVFX(Transform attacker)
    {
        if (damageVFXPrefab != null)
        {
            // Tạo Prefab tại vị trí của attacker
            GameObject vfx = Instantiate(damageVFXPrefab, attacker.position, Quaternion.identity);

            // Đặt Prefab trở thành con của Enemy
            vfx.transform.SetParent(transform);
        }
    }

    private int GetDefense()
    {
        return 0; // Có thể tùy chỉnh theo logic game
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
