using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;

    public float rotateSpeed = 10.0f;

    public Transform viewTarget;

    public GravitySystem gravity;


    private Vector3 GetPlanerVector(Vector3 vec)
    {
        vec.y = 0;
        return vec;
    }

    public void AlignWithGravity(Vector3 up, Vector3 forward)
    {
        StartCoroutine(AligningWithGravity(up, forward));
    }

    private IEnumerator AligningWithGravity(Vector3 up, Vector3 forward)
    {
        yield return null;
        // GetClampedForward
        transform.up = up;
        transform.forward = forward;
    }

    void Update()
    {
        Vector3 displacement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            displacement += GetPlanerVector(viewTarget.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            displacement += GetPlanerVector(-viewTarget.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            displacement += GetPlanerVector(-viewTarget.right);
        }
        if (Input.GetKey(KeyCode.D))
        {
            displacement += GetPlanerVector(viewTarget.right);
        }
        displacement.Normalize();
        if (displacement.magnitude > 0.0f)
        {
            Vector3 currentLookAt = GetPlanerVector(transform.forward).normalized;
            transform.forward = Vector3.RotateTowards(currentLookAt, displacement, rotateSpeed * Time.deltaTime, 0.0f).normalized;
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
