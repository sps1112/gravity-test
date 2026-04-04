using System;
using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    public Transform viewObject;
    public PlayerMovement player;
    public GameObject directionReference;
    public Transform reference;
    public Transform reference2;

    public static Vector3 GetClosestCardinalVector(Vector3 vec)
    {
        float ax = Mathf.Abs(vec.x);
        float ay = Mathf.Abs(vec.y);
        float az = Mathf.Abs(vec.z);
        float maxComponent = Mathf.Max(ax, Mathf.Max(ay, az));
        if (maxComponent == ax)
        {
            return (ax / vec.x > 0) ? Vector3.right : Vector3.left;
        }
        else if (maxComponent == ay)
        {
            return (ay / vec.y > 0) ? Vector3.up : Vector3.down;
        }
        else
        {
            return (az / vec.z > 0) ? Vector3.forward : Vector3.back;
        }
    }

    private Vector3 GetViewForward()
    {
        return GetClosestCardinalVector(viewObject.forward);
    }

    private Vector3 GetViewRight(Vector3 forward)
    {
        Vector3 right = Vector3.Cross(viewObject.up, forward);
        return GetClosestCardinalVector(right);
    }

    void Update()
    {
        Vector3 forward = GetViewForward();
        Vector3 right = GetViewRight(forward);
        Vector3 up = Vector3.Cross(forward, right);
        // reference.up = forward;
        // reference2.up = right;
        Quaternion lookRotation = Quaternion.identity;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            directionReference.SetActive(true);
            lookRotation = Quaternion.LookRotation(forward, -right);
            directionReference.transform.rotation = lookRotation;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            directionReference.SetActive(true);
            lookRotation = Quaternion.LookRotation(forward, right);
            directionReference.transform.rotation = lookRotation;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            directionReference.SetActive(true);
            lookRotation = Quaternion.LookRotation(-up, forward);
            directionReference.transform.rotation = lookRotation;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            directionReference.SetActive(true);
            lookRotation = Quaternion.LookRotation(up, -forward);
            directionReference.transform.rotation = lookRotation;
        }
        else
        {
            directionReference.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Return) && directionReference.activeSelf == true)
        {
            transform.rotation = lookRotation;
            player.AlignWithGravity(lookRotation);
        }
    }

    public Vector3 GetGravityUpDirection()
    {
        return transform.up;
    }
}
