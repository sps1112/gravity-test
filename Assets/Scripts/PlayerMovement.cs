using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;

    [SerializeField] private float turnSpeed = 10.0f;

    [SerializeField] private Transform viewTarget;

    [SerializeField] private GravitySystem gravity;

    private Animator animator;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Gives a Vector along the 2D plane which the player can move in
    private Vector3 ProjectOnGravityPlane(Vector3 vec)
    {
        return Vector3.ProjectOnPlane(vec, gravity.GetGravityUpDirection());
    }

    // Start rotating the player to align with the gravity direction
    public void AlignWithGravity(Quaternion finalRotation)
    {
        StopAllCoroutines();
        StartCoroutine(AligningWithGravity(finalRotation));
    }

    // Rotates the player over time to align with gravity direction
    private IEnumerator AligningWithGravity(Quaternion finalRotation)
    {
        while (Quaternion.Angle(transform.rotation, finalRotation) >= 10.0f ||
        Quaternion.Angle(viewTarget.rotation, finalRotation) >= 10.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * turnSpeed);
            viewTarget.rotation = Quaternion.Slerp(viewTarget.rotation, finalRotation, Time.deltaTime * turnSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = finalRotation;
        viewTarget.rotation = finalRotation;
    }

    void Update()
    {
        if (GameManager.canInput)
        {
            Vector3 displacement = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                displacement += ProjectOnGravityPlane(viewTarget.forward);
            if (Input.GetKey(KeyCode.S))
                displacement += ProjectOnGravityPlane(-viewTarget.forward);
            if (Input.GetKey(KeyCode.A))
                displacement += ProjectOnGravityPlane(-viewTarget.right);
            if (Input.GetKey(KeyCode.D))
                displacement += ProjectOnGravityPlane(viewTarget.right);

            if (displacement.magnitude > 0.0f)
            {
                animator.SetBool("IsMoving", true);

                displacement.Normalize();
                Quaternion finalRot = Quaternion.LookRotation(displacement, gravity.GetGravityUpDirection());
                transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, turnSpeed * Time.deltaTime);
                transform.position += moveSpeed * Time.deltaTime * transform.forward;
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}
