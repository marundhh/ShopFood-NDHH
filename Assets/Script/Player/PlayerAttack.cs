using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damage = 10; // Lượng sát thương gây ra cho Player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi enemy chạm vào player thì gây sát thương
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyStat enemy = collision.GetComponent<EnemyStat>();
            if (enemy != null && enemy.IsAlive())
            {
                enemy.TakeDamage(damage);
            }
        }
    }

}
