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
            Vector3 targetRotation = target.rotation.eulerAngles;
            targetRotation.y += Input.mousePositionDelta.x * mouseSensitivity * Time.deltaTime;
            targetRotation.x -= Input.mousePositionDelta.y * mouseSensitivity * Time.deltaTime;
            if (targetRotation.x > 180.0f)
            {
                targetRotation.x -= 360.0f;
            }
            targetRotation.x = Mathf.Clamp(targetRotation.x, -maxXRotation, maxXRotation);
            target.rotation = Quaternion.Euler(targetRotation);
        }
    }
}
