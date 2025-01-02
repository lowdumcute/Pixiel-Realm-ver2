using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class TowerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider; // Thanh Slider hiển thị máu
    public GameObject VFXDestroy;
    public GameObject VFXStart; // VFX cho khi Tower khởi động
    public GameObject VFXHit; // VFX cho khi Tower nhận sát thươngthương
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // Khởi tạo máu ban đầu
        healthSlider.gameObject.SetActive(false); // Ẩn thanh Slider ngay khi game bắt đầu
        UpdateHealthUI(); // Cập nhật giao diện UI lúc bắt đầu
        animator.SetBool("Lose", false);
        PlayStartVFX(new Vector3(0, 3, 0));
    }
    private void PlayStartVFX(Vector3 offset)
    {
        if (VFXStart != null)
    {
        // Thêm offset vào vị trí hiện tại của đối tượng
        Vector3 spawnPosition = transform.position + offset;

        // Tạo hiệu ứng VFX tại vị trí tính toán
        GameObject vfx = Instantiate(VFXStart, spawnPosition, Quaternion.identity);

        // Xoá hiệu ứng sau khi hoàn tất
        ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(vfx, 1f); // Dự phòng nếu không tìm thấy ParticleSystem
        }
    }
    }
    private void PlayDamageVFX(Vector3 offset)
{
    if (VFXHit != null)
    {
        // Thêm offset vào vị trí hiện tại của đối tượng
        Vector3 spawnPosition = transform.position + offset;

        // Tạo hiệu ứng VFX tại vị trí tính toán
        GameObject vfx = Instantiate(VFXHit, spawnPosition, Quaternion.identity);

        // Xoá hiệu ứng sau khi hoàn tất
        ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(vfx, 1f); // Dự phòng nếu không tìm thấy ParticleSystem
        }
    }
}


    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        PlayDamageVFX(new Vector3(0, 1, 0));
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
        gameObject.tag = "Untagged";
        animator.SetBool("Lose", true);
        if (VFXDestroy != null)
        {
            GameObject vfx = Instantiate(VFXDestroy, transform.position, Quaternion.identity);
            Destroy(vfx, 0.6f);
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false); // Ẩn các object con
        }
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
