using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Asset asset; // Tài sản chứa thông tin máu
    [SerializeField] private Flash flash;
    [SerializeField] private Slider healthSlider; // Thanh Slider hiển thị máu
    [SerializeField] private Animator animator; // Animator để điều khiển hiệu ứng
    [SerializeField] private float healRate = 0.05f; // Tỷ lệ hồi máu mỗi giây (5%)
    [SerializeField] private TMP_Text textRespawn; // Text hiển thị thời gian hồi sinh

    private int maxHealth; // Máu tối đa
    private int currentHealth; // Máu hiện tại
    private bool isHealing; // Trạng thái đang hồi máu

    private void Start()
    {
        animator.SetBool("Die", false);
        maxHealth = asset.Health;
        currentHealth = maxHealth;

        // Cấu hình Slider
        if (healthSlider != null)
        {
            UpdateHealthUI();
            healthSlider.gameObject.SetActive(true); // Hiển thị thanh máu
        }

        // Ẩn text hồi sinh ban đầu
        if (textRespawn != null)
        {
            textRespawn.gameObject.SetActive(false);
        }

        isHealing = false;
    }

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        if (animator.GetBool("Die")) return; // Không nhận sát thương khi đã chết

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo máu không vượt quá giới hạn
        UpdateHealthUI();

        // Kích hoạt hiệu ứng flash
        StartCoroutine(flash.FlashRountine());

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
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo không vượt quá máu tối đa
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
            healthSlider.value = Mathf.Clamp01((float)currentHealth / maxHealth); // Đảm bảo giá trị từ 0 đến 1
        }
    }

    // Xử lý khi người chơi chết
    private void Die()
    {
        animator.SetBool("Die", true);
        Debug.Log("Player has died!");
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false); // Ẩn thanh máu
        }

        if (textRespawn != null)
        {
            textRespawn.gameObject.SetActive(true); // Hiển thị text hồi sinh
        }

        StartCoroutine(Respawn());
    }

    // Hồi sinh sau 10 giây
    private IEnumerator Respawn()
    {
        int respawnTime = 10;

        // Đếm ngược thời gian hồi sinh
        while (respawnTime > 0)
        {
            if (textRespawn != null)
            {
                textRespawn.text = $"{respawnTime}";
            }
            yield return new WaitForSeconds(1f);
            respawnTime--;
        }

        // Ẩn text hồi sinh
        if (textRespawn != null)
        {
            textRespawn.gameObject.SetActive(false);
        }

        // Đặt lại máu đầy
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Hiện lại thanh máu
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
        }

        // Đặt trạng thái Die thành false
        animator.SetBool("Die", false);
        Debug.Log("Player has respawned!");
    }
}
