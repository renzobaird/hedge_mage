using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Animator animator;
    private bool canMove = true;
    private Vector2 lastMoveDirection = Vector2.down;
    private bool isBookOpen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("PlayerMovement: Animator component not found!");
        }
    }

    void Update()
    {
        HandleBookToggle();

        if (playerHealth != null && playerHealth.IsDead)
        {
            movement = Vector2.zero;

            if (animator != null)
            {
                animator.SetBool("isDead", true);
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", -1f);
                animator.SetFloat("Speed", 0f);
            }

            return;
        }

        if (animator != null)
        {
            animator.SetBool("isDead", false);
        }

        if (!canMove)
        {
            movement = Vector2.zero;
            UpdateAnimatorMovement(Vector2.zero);
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement.normalized;
        }

        UpdateAnimatorMovement(movement);
    }

    void FixedUpdate()
    {
        if (!canMove || (playerHealth != null && playerHealth.IsDead))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdateAnimatorMovement(Vector2 moveInput)
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
            animator.SetFloat("Speed", moveInput.sqrMagnitude);

            if (moveInput.sqrMagnitude > 0.01f)
            {
                animator.SetFloat("LastHorizontal", moveInput.x);
                animator.SetFloat("LastVertical", moveInput.y);
            }
            else
            {
                animator.SetFloat("LastHorizontal", lastMoveDirection.x);
                animator.SetFloat("LastVertical", lastMoveDirection.y);
            }
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled)
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            UpdateAnimatorMovement(Vector2.zero);
        }
    }

    void HandleBookToggle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBookOpen = !isBookOpen;

            if (isBookOpen)
            {
                LevelPopupManager.Instance.ShowBookPopup();
            }
            else
            {
                LevelPopupManager.Instance.CloseBookPopup();
            }
        }
    }
}
