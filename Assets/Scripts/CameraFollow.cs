using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Follow")]

    [SerializeField] private Transform target;

    [SerializeField] private Vector3 positionOffet;

    [Range(0.0f, 1.0f)]
    [Tooltip("Smoothness or Lerp factor for the camera")]
    [SerializeField] private float smoothness = 1.0f;

    [Header("Camera Rotate")]

    [SerializeField] private GravitySystem gravitySystem;

    [SerializeField] private float mouseSensitivity = 100.0f;

    [Range(1.0f, 89.0f)]
    [SerializeField] private float maxVerticalPitch = 85.0f;

    [Tooltip("Initial time to ignore mouse movements when game starts")]
    [SerializeField] private float flushMouseMovementTime = 0.5f;

    private bool canRotateViaMouse;

    void Start()
    {
        StartCoroutine(FlushMouseMovement());
    }

    private IEnumerator FlushMouseMovement()
    {
        canRotateViaMouse = false;
        yield return new WaitForSeconds(flushMouseMovementTime);
        canRotateViaMouse = true;
    }

    void LateUpdate()
    {
        // Set Position and Rotation w.r.t the Target
        Vector3 targetPos = target.TransformPoint(positionOffet);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothness);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, smoothness);

        if (GameManager.canInput && canRotateViaMouse)
        {
            // Process Mouse Movement along X Axis or Yaw Rotation
            float horizontalRot = Input.mousePositionDelta.x * mouseSensitivity * Time.deltaTime;
            Vector3 gravityUp = gravitySystem.GetGravityUpDirection();
            target.Rotate(gravityUp, horizontalRot, Space.World);

            // Process Mouse Movement along Y Axis or Pitch Rotation
            float verticalRot = Input.mousePositionDelta.y * mouseSensitivity * Time.deltaTime;
            Vector3 verticalAxis = Vector3.Cross(gravityUp, target.forward);
            if (Mathf.Approximately(verticalAxis.sqrMagnitude, 0.0f))
            {
                verticalAxis = target.right;
            }
            verticalAxis.Normalize();
            target.Rotate(verticalAxis, -verticalRot, Space.World);

            // After rotation get the resultant pitch from vectors and clamp it
            Vector3 forward = target.forward;
            float angleUp = Vector3.Angle(forward, gravityUp);
            // Find extreme angles w.r.t 90 as the horizon angle and clamp
            float minAngleUp = Mathf.Clamp(90f - maxVerticalPitch, 1f, 179f);
            float maxAngleUp = Mathf.Clamp(90f + maxVerticalPitch, 1f, 179f);
            float clampedAngleUp = Mathf.Clamp(angleUp, minAngleUp, maxAngleUp);

            // Apply the new angle if different
            if (!Mathf.Approximately(angleUp, clampedAngleUp))
            {
                // The new forward is a certain degrees from the up vector. Thus we can just add the components to get it
                // We have the up vector, we just need the horizontal axis

                // Get the horizontal axis
                Vector3 horizontal = Vector3.ProjectOnPlane(forward, gravityUp);
                // Check for extreme cases for rotation axis
                if (Mathf.Approximately(horizontal.sqrMagnitude, 0.0f))
                {
                    horizontal = Vector3.ProjectOnPlane(target.right, gravityUp);
                    if (Mathf.Approximately(horizontal.sqrMagnitude, 0.0f))
                        horizontal = Vector3.ProjectOnPlane(Vector3.forward, gravityUp);
                }
                horizontal.Normalize();

                // V = Up * Cos(rad) + Horizontal * Sin (rad)
                float rad = clampedAngleUp * Mathf.Deg2Rad;
                Vector3 newForward = Mathf.Sin(rad) * horizontal + Mathf.Cos(rad) * gravityUp;
                target.rotation = Quaternion.LookRotation(newForward.normalized, gravityUp);
            }
        }
    }
}
