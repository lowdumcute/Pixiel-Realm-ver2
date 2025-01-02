using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Asset asset; // Tài sản chứa thông tin máu
    [SerializeField] private Flash flashEffect; // Hiệu ứng flash khi nhận sát thương
    [SerializeField] private Slider healthSlider; // Thanh Slider hiển thị máu
    [SerializeField] private Animator animator; // Animator để điều khiển hiệu ứng
    [SerializeField] private float healRate = 0.05f; // Tỷ lệ hồi máu mỗi giây (5%)
    [SerializeField] private TMP_Text textRespawn; // Text hiển thị thời gian hồi sinh

    private int maxHealth; // Máu tối đa
    private int currentHealth; // Máu hiện tại
    private bool isHealing; // Trạng thái đang hồi máu

    private void Start()
    {
        textRespawn.gameObject.SetActive(false); // Ẩn text hồi sinh
        animator.SetBool("Die", false);
        maxHealth = asset.Health;
        currentHealth = maxHealth;
        healthSlider.gameObject.SetActive(true); // Hiển thị thanh máu

        isHealing = false;
        UpdateHealthUI();
    }

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Đảm bảo máu không âm
        Debug.Log($"Player took damage: {damage}. Current health: {currentHealth}");
        UpdateHealthUI();

        if (flashEffect != null)
        {
            StartCoroutine(flashEffect.FlashRountine());
        }

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
        gameObject.tag = "Untagged"; // Đổi tag thành "Untagged"
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
        gameObject.tag = "Player"; // Đổi tag thành "Player"
        Debug.Log("Player has respawned!");
    }
}
