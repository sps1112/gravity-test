using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;

    public float rotateSpeed = 10.0f;

    public Transform viewTarget;

    public GravitySystem gravity;

    public Transform reference;
    public Transform reference2;

    private Vector3 GetPlanerVector(Vector3 vec, Vector3 up)
    {
        Vector3 cardinalUp = GravitySystem.GetClosestCardinalVector(up);
        return Vector3.ProjectOnPlane(vec, cardinalUp);
    }

    public void AlignWithGravity(Quaternion finalRotation)
    {
        StopAllCoroutines();
        StartCoroutine(AligningWithGravity(finalRotation));
    }

    private IEnumerator AligningWithGravity(Quaternion finalRotation)
    {
        while (Quaternion.Angle(transform.rotation, finalRotation) >= 10.0f)
        {
            Debug.Log("Rotating player");
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * rotateSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = finalRotation;

        while (Quaternion.Angle(viewTarget.rotation, finalRotation) >= 10.0f)
        {
            Debug.Log("Rotating view");
            viewTarget.rotation = Quaternion.Slerp(viewTarget.rotation, finalRotation, Time.deltaTime * rotateSpeed);
            yield return new WaitForEndOfFrame();
        }
        viewTarget.rotation = finalRotation;
    }

    void Update()
    {
        Vector3 displacement = Vector3.zero;
        reference.up = viewTarget.forward;
        reference2.up = viewTarget.right;
        if (Input.GetKey(KeyCode.W))
        {
            displacement += GetPlanerVector(viewTarget.forward, viewTarget.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            displacement += GetPlanerVector(-viewTarget.forward, viewTarget.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            displacement += GetPlanerVector(-viewTarget.right, viewTarget.up);
        }
        if (Input.GetKey(KeyCode.D))
        {
            displacement += GetPlanerVector(viewTarget.right, viewTarget.up);
        }
        displacement.Normalize();
        if (displacement.magnitude > 0.0f)
        {
            // Vector3 currentLookAt = GetPlanerVector(transform.forward, transform.up).normalized;
            Quaternion finalRot = Quaternion.LookRotation(displacement, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, rotateSpeed * Time.deltaTime);
            // Debug.Log(currentLookAt + " " + displacement + " " + transform.forward);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
