using UnityEngine;

public class PositionSync : MonoBehaviour
{
    public Transform syncTarget;

    public Vector3 positionOffset = Vector3.zero;

    void Update()
    {
        transform.position = syncTarget.position + positionOffset;
    }
}
