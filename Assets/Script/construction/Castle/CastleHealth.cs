using System;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] private Asset asset; // Tài sản chứa thông tin Castle
    private int maxHealth; // Máu tối đa
    public int currentHealth; // Máu hiện tại
    public Slider healthSlider; // Thanh Slider hiển thị máu
    [SerializeField] private GamePlayManager gamePlayManager;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        maxHealth = asset.HealthCastle; // Gán giá trị máu tối đa từ Asset
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
        animator.SetTrigger("Lose");
        gamePlayManager.Lose(); // Kích hoạt Lose Panel và dừng game

        // Ẩn toàn bộ các object con
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        healthSlider.gameObject.SetActive(false); // Ẩn thanh Slider
    }

    // Hàm được gọi khi tiêu diệt quái (Ẩn Slider)
    public void OnEnemyDefeated()
    {
        healthSlider.gameObject.SetActive(false);
    }

    // Phục hồi máu đầy đủ
    public void RestoreHealth()
    {
        currentHealth = maxHealth; // Khôi phục máu đầy
        UpdateHealthUI();
        healthSlider.gameObject.SetActive(false); // Ẩn thanh Slider
    }
}
