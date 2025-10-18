using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
   
    public float moveSpeed = 5f;        
    public float rotationSpeed = 10f;   
    public float jumpForce = 5f;        
    [Header("Dash-instellingen")]
    public float dashForce = 15f;       
    public float dashDuration = 0.2f;   
    public float dashCooldown = 1f;     

    private Rigidbody rb;
    private Animator animator;
    private Vector3 moveDirection;
    private bool isGrounded;
    private bool isDashing = false;
    private float lastDashTime = -10f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            Debug.Log("Springt");
            animator.SetTrigger("Jump");
            animator.SetBool("IsGrounded", isGrounded);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(PerformDash());
        }

        animator.SetFloat("Speed", moveDirection.magnitude);
        


    }

    void FixedUpdate()
    {
        if (isDashing) return;

        Vector3 move = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private System.Collections.IEnumerator PerformDash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        animator.SetTrigger("Dash");

        Vector3 dashDir;
        if (moveDirection.magnitude > 0.1f)
        {
            dashDir = moveDirection;
        }
        else
        {
            dashDir = transform.forward;
        }

        rb.useGravity = false;
        rb.AddForce(dashDir * dashForce, ForceMode.VelocityChange);

        Debug.Log("Dash");

        yield return new WaitForSeconds(dashDuration);
        rb.useGravity = true;
        isDashing = false;
    }

    // --- Gronddetectie ---
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Raakt: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grond geraakt!");
            isGrounded = true;
            animator.SetBool("IsGrounded", true);   
        }
    }
}
