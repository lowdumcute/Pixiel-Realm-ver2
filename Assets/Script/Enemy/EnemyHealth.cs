using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    private Flash flash;
    private bool isDead = false;
    private bool isBeingAttacked = false;

    // Tham chiếu tới EnemyHealthBar để cập nhật thanh máu
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private Enemy enemy;
    public KillEnemiesProgress killEnemiesProgress;

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

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // Gọi FindTarget từ instance cụ thể của Enemy
        currentHealth -= Mathf.Max(damage - GetDefense(), 0); // Đảm bảo máu không dưới 0
        isBeingAttacked = true;
        StartCoroutine(flash.FlashRountine());

        Debug.Log("Enemy takes damage: " + damage + ", Current Health: " + currentHealth);

        // Cập nhật thanh máu
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

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
        FindObjectOfType<SpawnManager>().OnEnemyDefeated();
        killEnemiesProgress.OnEnemyKilled();
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
