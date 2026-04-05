using System.Collections;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Gravity")]

    [SerializeField] private GravitySystem gravity;

    [Tooltip("Gravity when falling. Increase for faster descends. 1.0 means normal gravity")]
    [SerializeField] private float fallGravityFactor = 1.0f;

    [SerializeField] private float terminalFallSpeed = 10.0f;

    private float verticalSpeed = 0.0f;

    [Header("Ground Check")]

    [Tooltip("Distance of Ground Check Ray from player's center")]
    [SerializeField] private float groundCheckDistance = 0.2f;

    private bool isOnGround = false;

    private bool canCheckForGround = true;

    [Header("Jump Mechanic")]

    [SerializeField] private float jumpSpeed = 15.0f;

    [Tooltip("Cooldown time on ground checks after jump")]
    [SerializeField] private float groundCheckTime = 0.25f;

    private Animator animator;

    [Header("Game Over")]

    [Tooltip("Maximum distance the player reaches from the map for game over")]
    [SerializeField] private float maxDistance;

    private Vector3 startOrigin;

    void Start()
    {
        startOrigin = transform.position;
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Checks if the player has fallen too far from the map, then game over
    private void CheckForFreeFall()
    {
        if (Vector3.SqrMagnitude(transform.position - startOrigin) > maxDistance * maxDistance)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
    }

    // Player Jumps and Ground Check starts its cooldown
    private void Jump()
    {
        StopAllCoroutines();
        StartCoroutine(RefreshGroundCheck());
        verticalSpeed = jumpSpeed;
    }

    private IEnumerator RefreshGroundCheck()
    {
        canCheckForGround = false;
        yield return new WaitForSeconds(groundCheckTime);
        canCheckForGround = true;
    }

    void FixedUpdate()
    {
        if (canCheckForGround)
        {
            Vector3 down = -gravity.GetGravityUpDirection();
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, down * groundCheckDistance, Color.black, 0.01f);
#endif
            isOnGround = Physics.Raycast(transform.position, down, groundCheckDistance, LayerMask.GetMask("Ground"));
            animator.SetBool("IsOnGround", isOnGround);
        }
    }

    void Update()
    {
        CheckForFreeFall();

        Vector3 up = gravity.GetGravityUpDirection();
        float g = -Physics.gravity.magnitude; // -9.8

        if (isOnGround)
        {
            verticalSpeed = 0.0f;
            if (Input.GetKeyDown(KeyCode.Space) && GameManager.canInput)
            {
                isOnGround = false;
                animator.SetBool("IsOnGround", isOnGround);
                Jump();
            }
        }
        else
        {
            if (verticalSpeed > 0.0f && Input.GetKeyUp(KeyCode.Space))
                verticalSpeed = 0.0f;
            if (verticalSpeed > 0.0f)
                verticalSpeed += g * Time.deltaTime;
            else
                verticalSpeed += fallGravityFactor * g * Time.deltaTime;
        }
        verticalSpeed = Mathf.Clamp(verticalSpeed, -terminalFallSpeed, jumpSpeed);
        transform.position += up * verticalSpeed * Time.deltaTime;
    }
}
