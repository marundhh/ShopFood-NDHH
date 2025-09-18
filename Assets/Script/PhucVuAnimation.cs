using UnityEngine;

public class PhucVuAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayIdle() => animator?.SetTrigger("Idle");
    public void PlayCarry() => animator?.SetTrigger("Carry");
    public void PlayReturn() => animator?.SetTrigger("Return");
}
