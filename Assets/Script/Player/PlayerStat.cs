using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerStat : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 100;          // Máu tối đa
    public int currentHealth;            // Máu hiện tại

    [Header("UI References")]
    public Slider healthBar;             // Thanh máu hiển thị trên UI (nếu có)
         // Hiệu ứng chết (tùy chọn)

    private PlayerAnimationController animController;
    private bool isDead = false;

    void Start()
    {
        animController = GetComponent<PlayerAnimationController>();
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    /// <summary>
    /// Gọi hàm này khi player bị tấn công.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Cập nhật UI
        if (healthBar != null)
            healthBar.value = currentHealth;

        // Gọi animation bị đánh
        animController.Hit();

        // Kiểm tra chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Gọi khi player chết.
    /// </summary>
    private void Die()
    {
        isDead = true;
        animController.Die();


        // Ngừng di chuyển / tấn công
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

        // Có thể disable input hoặc script khác
        GetComponent<PlayerAnimationController>().enabled = false;

        // Tùy chọn: destroy sau vài giây
        Destroy(gameObject, 4f);
    }

    /// <summary>
    /// Gọi khi muốn hồi máu (ví dụ: dùng bình máu).
    /// </summary>
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    /// <summary>
    /// Kiểm tra player còn sống hay không.
    /// </summary>
    public bool IsAlive()
    {
        return !isDead;
    }
}
