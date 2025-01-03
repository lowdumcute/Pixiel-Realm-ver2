using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    private Flash flash;
    private bool isDead = false;


    [SerializeField] private WariorHealthBar healthBar;
    [SerializeField] private GameObject healthbarobj;

    public void Start()
    {
        flash = GetComponent<Flash>();
        Initialize();
    }

    public void Initialize()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth); // Cập nhật thanh máu ngay từ đầu
        }
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
        healthbarobj.SetActive(false);
        isDead = false;
        gameObject.SetActive(true); // Đảm bảo object được kích hoạt khi reset
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        healthbarobj.SetActive(true);
        currentHealth -= Mathf.Max(damage - GetDefense(), 0); // Đảm bảo máu không dưới 0
        StartCoroutine(flash.FlashRountine());

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
        isDead = true;
        gameObject.SetActive(false);
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
