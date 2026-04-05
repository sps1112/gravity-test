using UnityEngine;

public class PositionSync : MonoBehaviour
{
    [SerializeField] private Transform syncTarget;

    [SerializeField] private Vector3 positionOffset = Vector3.zero;

    void Update()
    {
        transform.position = syncTarget.TransformPoint(positionOffset);
    }
}
