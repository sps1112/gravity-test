using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    public Transform viewObject;

    public PlayerMovement player;

    public GameObject directionReference;

    public Transform reference;
    public Transform reference2;

    private Vector3 GetClosestCardinalVector(Vector3 vec)
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
        reference.up = forward;
        reference2.up = right;
        int option = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            directionReference.SetActive(true);
            directionReference.transform.up = -right;
            option = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            directionReference.SetActive(true);
            directionReference.transform.up = right;
            option = 2;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            directionReference.SetActive(true);
            directionReference.transform.up = forward;
            option = 3;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            directionReference.SetActive(true);
            directionReference.transform.up = -forward;
            option = 4;
        }
        else
        {
            directionReference.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Return) && option != 0)
        {

            Debug.Log(forward);
            switch (option)
            {
                case 1:
                    transform.Rotate(-forward, 90.0f);
                    break;
                case 2:
                    transform.Rotate(forward, 90.0f);
                    break;
                case 3:
                    transform.Rotate(-right, 90.0f);
                    break;
                case 4:
                    transform.Rotate(right, 90.0f);
                    break;
            }
        }
    }

    public Vector3 GetGravityUpDirection()
    {
        return transform.up;
    }
}
