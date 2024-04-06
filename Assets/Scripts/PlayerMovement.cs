using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 10f;

    private Vector2 moveInput;
    private bool isPlayerMoving = false;
    private bool canJump = true;
    private bool canMove = true;

    private Rigidbody2D myRigidBody;
    private CapsuleCollider2D myFeetCollider;
    private Animator myAnimator;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canMove)
        {
            CheckIfPlayerIsMoving();
            FlipCharacter();
            Move();
        }
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!canJump || !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            // Don't jump when not touching ground or not evolved to jump
            return;
        }

        myRigidBody.velocity += new Vector2(0f, jumpPower);
    }

    private void Move()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }

    private void FlipCharacter()
    {
        if (isPlayerMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
        }
    }

    private void CheckIfPlayerIsMoving()
    {
        isPlayerMoving = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        myAnimator.SetBool("isWalking", isPlayerMoving);
    }

    public void CanJump(bool canJumpValue)
    {
        canJump = canJumpValue;
    }

    public void SetMoveSpeed(float moveSpeedValue)
    {
        moveSpeed = moveSpeedValue;
    }

    public void PreventPlayerMovement()
    {
        canMove = false;
    }
}
