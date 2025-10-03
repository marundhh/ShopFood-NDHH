using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Move Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    private bool facingRight = true;
    private bool isAttacking = false;
    private GameObject targetPlayer;

    [Header("Attack Settings")]
    public BoxCollider2D attackCheck;   // collider con dùng làm vùng check
    public LayerMask playerLayer;

    [Header("Animation")]
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // đảm bảo attackCheck là trigger
        if (attackCheck != null) attackCheck.isTrigger = true;
    }

    void Update()
    {
        if (!isAttacking)
        {
            Patrol();
        }
        else
        {
            ChaseAndAttack();
        }
    }

    void Patrol()
    {
        rb.velocity = new Vector2((facingRight ? 1 : -1) * walkSpeed, rb.velocity.y);
        animator.SetBool("walk", true);

        // kiểm tra hết ground thì quay đầu
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        if (!groundInfo.collider)
        {
            Flip();
        }
    }

    void ChaseAndAttack()
    {
        if (targetPlayer == null) return;

        float direction = targetPlayer.transform.position.x - transform.position.x;
        rb.velocity = new Vector2(Mathf.Sign(direction) * runSpeed, rb.velocity.y);

        animator.SetBool("walk", false);
        animator.SetTrigger("attack");

        // tự động quay mặt theo hướng player
        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // --- Trigger vùng attack ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            targetPlayer = collision.gameObject;
            isAttacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            targetPlayer = null;
            isAttacking = false;
            animator.ResetTrigger("attack");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
}
