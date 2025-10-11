using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyStat : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 100;              // Máu tối đa
    public int currentHealth;                // Máu hiện tại
    public GameObject deathEffect;           // Hiệu ứng chết (tùy chọn)

    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Gọi khi enemy bị trúng đòn.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Phát animation bị đánh
        animator.SetTrigger("Hit");

        // Kiểm tra chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Gọi khi enemy chết.
    /// </summary>
    private void Die()
    {
        isDead = true;

        // Dừng mọi chuyển động
        if (rb != null)
            rb.velocity = Vector2.zero;

        // Phát animation chết
        animator.SetTrigger("Death");

        // Gọi hiệu ứng chết (nếu có)
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Tắt collider để không va chạm nữa
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Vô hiệu hóa AI hoặc script điều khiển
        EnemyController controller = GetComponent<EnemyController>();
        if (controller != null)
            controller.enabled = false;

        // Hủy enemy sau vài giây (cho animation chạy xong)
        Destroy(gameObject);
    }

    /// <summary>
    /// Hồi máu cho enemy (nếu cần, ví dụ kẻ địch đặc biệt).
    /// </summary>
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    /// <summary>
    /// Kiểm tra còn sống hay không.
    /// </summary>
    public bool IsAlive()
    {
        return !isDead;
    }
}
