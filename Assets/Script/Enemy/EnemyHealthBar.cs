using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    // Phương thức cập nhật thanh máu
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }
}
