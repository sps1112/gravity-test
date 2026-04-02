using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Gravity")]
    public float terminalFallSpeed = 10.0f;
    public float verticalSpeed = 0.0f;
    public float fallGravityFactor = 1.0f;
    public bool isOnGround = false;
    public float checkOffset = 2.0f;
    public float groundCheckDistance = 0.2f;
    public float jumpSpeed = 15.0f;
    public bool canCheckForGround = true;
    public float groundCheckTime = 0.25f;
    private void Jump()
    {
        StopAllCoroutines();
        StartCoroutine(RefreshGroundCheck());
        Debug.Log("Jumped");
        verticalSpeed = jumpSpeed;
    }

    void FixedUpdate()
    {
        if (canCheckForGround)
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + Vector3.up * checkOffset,
             -transform.up * (groundCheckDistance + checkOffset), Color.black, 0.01f);
#endif

            if (Physics.Raycast(transform.position + Vector3.up * checkOffset,
             -transform.up, groundCheckDistance + checkOffset, LayerMask.GetMask("Ground")))
            {
                Debug.Log("On Ground");
                isOnGround = true;
            }
            else
            {
                Debug.Log("In Air");
                isOnGround = false;
            }
        }
    }

    private IEnumerator RefreshGroundCheck()
    {
        canCheckForGround = false;
        yield return new WaitForSeconds(groundCheckTime);
        canCheckForGround = true;
    }

    void Update()
    {
        if (isOnGround)
        {
            verticalSpeed = 0.0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isOnGround = false;
                Jump();
            }
        }
        else
        {
            if (verticalSpeed > 0.0f)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    verticalSpeed = 0.0f;
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    verticalSpeed += Physics.gravity.y * Time.deltaTime;
                }
            }
            else
            {
                verticalSpeed += fallGravityFactor * Physics.gravity.y * Time.deltaTime;
            }
        }
        verticalSpeed = Mathf.Clamp(verticalSpeed, -terminalFallSpeed, jumpSpeed);
        transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
    }
}
