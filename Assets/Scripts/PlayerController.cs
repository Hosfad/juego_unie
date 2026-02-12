using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Animator animator;
    [SerializeField] private string modelChildName = "ty";

    [Header("State")]
    public bool isFalling = false; 

    private CharacterController controller;
    private float turnSmoothVelocity;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    
    private int animIDGrounded;
    private int animIDRunning;
    private int animIDIdling;
    private int animIDJumping;
    private int animIDFalling;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        // TODO: Add is grounded and is falling animations

        animIDGrounded = Animator.StringToHash("isGrounded");
        animIDRunning = Animator.StringToHash("isRunning");
        animIDIdling = Animator.StringToHash("isIdling");
        animIDJumping = Animator.StringToHash("isJumping");
        animIDFalling = Animator.StringToHash("isFalling");
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialPosition.y += 50; 

        if (animator == null)
        {
            Transform model = transform.Find(modelChildName);
            if (model != null) animator = model.GetComponent<Animator>();
            else Debug.LogError($"Could not find child object '{modelChildName}' with an Animator.");
        }
    }

    private void Update()
    {
        Vector2 input = GetInput();

        HandleMovement(input);

        HandleGravityAndJump();

        controller.Move(moveDirection * Time.deltaTime);

        UpdateAnimator(input);
        CheckRespawn();
    }

    private Vector2 GetInput()
    {
        if (isFalling) return Vector2.zero;
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void HandleMovement(Vector2 input)
    {
        if (input.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            moveDirection.x = moveDir.x * moveSpeed;
            moveDirection.z = moveDir.z * moveSpeed;
        }
        else
        {
            moveDirection.x = 0f;
            moveDirection.z = 0f;
        }
    }

    private void HandleGravityAndJump()
    {
        if (controller.isGrounded)
        {
            if (animator) animator.SetBool(animIDFalling, false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
                if (animator)
                {
                    animator.SetTrigger(animIDJumping);
                    animator.SetBool(animIDRunning, false);
                    animator.SetBool(animIDIdling, false);
                }
            }
        }

        moveDirection.y += gravity * Time.deltaTime;
    }

    private void UpdateAnimator(Vector2 input)
    {
        if (animator == null) return;

        animator.SetBool(animIDGrounded, controller.isGrounded);

        if (controller.isGrounded)
        {
            bool isMoving = input.sqrMagnitude > 0.01f;
            animator.SetBool(animIDRunning, isMoving);
            animator.SetBool(animIDIdling, !isMoving);
        }
        else
        {
            animator.SetBool(animIDRunning, false);
            animator.SetBool(animIDIdling, false);
        }
    }

    private void CheckRespawn()
    {
        if (transform.position.y < -30)
        {
            controller.enabled = false; 
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            controller.enabled = true;

            if (animator) animator.SetBool(animIDFalling, true);
        }
    }
}