using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    private bool isGrounded;

    [Header("Roll Settings")]
    public float rollSpeed = 6f;
    public float rollDuration = 0.4f;
    private bool isRolling = false;

    private bool isAttackingUninterruptible = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGrounded();

        if (!isAttackingUninterruptible && !isRolling)
        {
            HandleMovement();
            HandleJump();
        }

        HandleAttack();
        HandleRoll();
    }

    void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        // Flip hướng theo input
        if (move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        // Chỉ set tốc độ cho Animator
        animator.SetFloat("Speed", Mathf.Abs(move));
    }

    void FixedUpdate()
    {
        if (!isRolling && !isAttackingUninterruptible)
        {
            float move = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }
    }

    void HandleJump()
    {
        // Nhảy khi ở trên mặt đất
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }

        // Trạng thái trên không
        if (!isGrounded)
        {
            if (rb.velocity.y > 0) // đang bay lên
            {
                animator.SetBool("JumpDown", false);
            }
            else if (rb.velocity.y < 0) // đang rơi xuống
            {
                animator.SetBool("JumpDown", true);
            }
        }
        else
        {
            animator.SetBool("JumpDown", false);
        }
    }

    void HandleAttack()
    {
        // Attack 1 (có thể bị ngắt)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("Atk1");
            isAttackingUninterruptible = false;
        }

        // Attack 2-3-4 (không bị ngắt giữa chừng)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(DoUninterruptibleAttack("Atk2"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(DoUninterruptibleAttack("Atk3"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(DoUninterruptibleAttack("Atk4"));
        }
    }

    System.Collections.IEnumerator DoUninterruptibleAttack(string triggerName)
    {
        isAttackingUninterruptible = true;
        animator.SetTrigger(triggerName);

        // Chờ hết animation state hiện tại
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAttackingUninterruptible = false;
    }

    void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling && isGrounded)
        {
            StartCoroutine(DoRoll());
        }
    }

    System.Collections.IEnumerator DoRoll()
    {
        isRolling = true;
        animator.SetTrigger("Roll");

        float dir = transform.localScale.x;
        float timer = 0f;

        while (timer < rollDuration)
        {
            rb.velocity = new Vector2(dir * rollSpeed, rb.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = new Vector2(0, rb.velocity.y);
        isRolling = false;
    }

    void CheckGrounded()
    {
        // Kiểm tra va chạm mặt đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Cập nhật cho Animator
        animator.SetBool("Grounded", isGrounded);
    }

    // Gọi từ script khác khi bị đánh hoặc chết
    public void Hit() => animator.SetTrigger("Hit");
    public void Die() => animator.SetTrigger("Death");
}
