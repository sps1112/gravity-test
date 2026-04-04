using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 positionOffet;

    [Range(0.0f, 1.0f)]
    public float followFactor = 1.0f;

    public float mouseSensitivity = 100.0f;

    public float maxXRotation = 10.0f;

    public float flushMouseMovementTime = 0.5f;

    public bool canRotateViaMouse = false;

    public GravitySystem gravitySystem;

    void Start()
    {
        StartCoroutine(FlushMouseMovement());
    }

    private IEnumerator FlushMouseMovement()
    {
        yield return new WaitForSeconds(flushMouseMovementTime);
        canRotateViaMouse = true;
        Debug.Log("Can now rotate via mouse");
    }


    void LateUpdate()
    {
        Vector3 targetPos = target.TransformPoint(positionOffet);
        transform.position = Vector3.Lerp(transform.position, targetPos, followFactor);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, followFactor);

        if (canRotateViaMouse)
        {
            float horizontalRot = Input.mousePositionDelta.x * mouseSensitivity * Time.deltaTime;
            Vector3 up = gravitySystem.GetGravityUpDirection();
            target.Rotate(up, horizontalRot, Space.World);


            // float verticalRot = Input.mousePositionDelta.y * mouseSensitivity * Time.deltaTime;
            // target.Rotate(target.right, verticalRot);

            // Vector3 targetForward = target.forward;
            // Vector3 localRight = Vector3.Cross(up, targetForward);
            // float angle = Vector3.SignedAngle(up, targetForward, localRight);

            // angle += Input.mousePositionDelta.y * mouseSensitivity * Time.deltaTime;
            // Debug.Log(angle);
            // Vector3 newForward=

            // Vector3 forward = GravitySystem.GetClosestCardinalVector(target.forward);
            // Vector3 right = Vector3.Cross(up, forward);
            // Debug.Log(up + " " + forward + " " + right);
            // target.Rotate(right, verticalRot);

            // Vector3 targetRotation = target.rotation.eulerAngles;
            // targetRotation.y +=
            // targetRotation.x -=
            // if (targetRotation.x > 180.0f)
            // {
            //     targetRotation.x -= 360.0f;
            // }
            // targetRotation.x = Mathf.Clamp(targetRotation.x, -maxXRotation, maxXRotation);
            // target.rotation = Quaternion.Euler(targetRotation);
        }
    }
}
