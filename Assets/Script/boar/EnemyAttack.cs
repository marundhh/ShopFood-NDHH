using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damage = 10; // Lượng sát thương gây ra cho Player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi enemy chạm vào player thì gây sát thương
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerStat player = collision.GetComponent<PlayerStat>();
            if (player != null && player.IsAlive())
            {
                player.TakeDamage(damage);
            }
        }
    }
}
