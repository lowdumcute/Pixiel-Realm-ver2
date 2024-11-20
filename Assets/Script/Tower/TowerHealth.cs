using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
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
        animator.SetBool("Lose", false);
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
        animator.SetBool("Lose", true);

        // Đổi tag của Tower thành "Untagged"
        gameObject.tag = "Untagged";

        // Ẩn tất cả các object con
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // Ẩn thanh máu
        healthSlider.gameObject.SetActive(false);

        // Thông báo cho tất cả các Enemy cập nhật mục tiêu
        Enemy.NotifyAllEnemiesToFindTarget();
    }

    // Hàm khôi phục lại máu cho Tower
    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        animator.SetBool("Lose", false);
        healthSlider.gameObject.SetActive(false);
        gameObject.tag = "Tower"; // Đặt lại tag
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true); // Hiển thị các object con
        }
    }
}
