using System;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider; // Thanh Slider hiển thị máu
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // Khởi tạo máu ban đầu
        healthSlider.gameObject.SetActive(false); // Ẩn thanh Slider ngay khi game bắt đầu
        UpdateHealthUI(); // Cập nhật giao diện UI lúc bắt đầu
    }

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Giảm máu
        currentHealth = Mathf.Max(currentHealth, 0); // Đảm bảo máu không giảm xuống dưới 0
        UpdateHealthUI(); // Cập nhật giao diện UI sau khi nhận sát thương

        // Hiển thị thanh Slider khi nhận sát thương
        if (!healthSlider.gameObject.activeSelf)
        {
            healthSlider.gameObject.SetActive(true);
        }

        if (currentHealth <= 0) // Nếu máu bằng 0 thì Tower bị phá hủy
        {
            Die();
        }
    }

    // Cập nhật giá trị thanh Slider
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth; // Cập nhật thanh Slider dựa trên tỉ lệ máu hiện tại

            // Ẩn thanh Slider khi máu đầy
            if (currentHealth == maxHealth && healthSlider.gameObject.activeSelf)
            {
                healthSlider.gameObject.SetActive(false);
            }
        }
    }

    // Hàm xử lý khi Tower bị phá hủy
    private void Die()
    {
        Debug.Log("Tower has been destroyed!");
        animator.SetTrigger("Lose");

        // Đổi tag của Tower thành "Untagged"
        gameObject.tag = "Untagged";

        // Ẩn tất cả các object con
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // Ẩn thanh máu
        healthSlider.gameObject.SetActive(false);
    }

    // Hàm được gọi khi tiêu diệt quái (Ẩn Slider)
    public void OnEnemyDefeated()
    {
        // Ẩn thanh Slider khi quái bị tiêu diệt
        healthSlider.gameObject.SetActive(false);
    }
    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        healthSlider.gameObject.SetActive(false);
    }
}
