using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Asset asset; // Tài sản chứa thông tin máu
    [SerializeField] private Flash flashEffect; // Hiệu ứng flash khi nhận sát thương
    [SerializeField] private Slider healthSlider; // Thanh Slider hiển thị máu
    [SerializeField] private Animator animator; // Animator để điều khiển hiệu ứng
    [SerializeField] private float healRate = 0.05f; // Tỷ lệ hồi máu mỗi giây (5%)

    private int maxHealth; // Máu tối đa
    private int currentHealth; // Máu hiện tại
    private bool isHealing; // Trạng thái đang hồi máu

    private void Start()
    {
        maxHealth = asset.Health;
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        isHealing = false;
        UpdateHealthUI();
    }

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Đảm bảo máu không âm
        UpdateHealthUI();

        // Kích hoạt hiệu ứng flash
        if (flashEffect != null)
        {
            StartCoroutine(flashEffect.FlashRountine());
        }

        // Kiểm tra chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Hồi máu
    private IEnumerator HealOverTime()
    {
        isHealing = true;
        while (currentHealth < maxHealth)
        {
            currentHealth += Mathf.CeilToInt(maxHealth * healRate); // Hồi 5% máu tối đa
            currentHealth = Mathf.Min(currentHealth, maxHealth); // Đảm bảo không vượt quá máu tối đa
            UpdateHealthUI();

            if (currentHealth == maxHealth)
            {
                break; // Dừng khi máu đầy
            }

            yield return new WaitForSeconds(1f); // Hồi máu mỗi giây
        }
        isHealing = false;
    }

    // Cập nhật giao diện thanh máu
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }

        // Tự động bắt đầu hồi máu nếu không đầy
        if (currentHealth < maxHealth && !isHealing)
        {
            StartCoroutine(HealOverTime());
        }
    }

    // Xử lý khi người chơi chết
    private void Die()
    {
        animator.SetTrigger("Die");
        // Thêm logic xử lý game over, ví dụ: dừng game, hiển thị UI...
        Debug.Log("Player has died!");
    }
}
