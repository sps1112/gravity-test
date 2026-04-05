using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    [Tooltip("Player object reference to decide directions for gravity switch")]
    [SerializeField] private Transform playerRef;

    [Tooltip("Hologram which shows the new gravity orientation")]
    [SerializeField] private GameObject hologramRef;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = playerRef.GetComponent<PlayerMovement>();
    }

    // Returns the closest cardinal vector to the current vector
    public static Vector3 GetClosestCardinalVector(Vector3 vec)
    {
        float ax = Mathf.Abs(vec.x);
        float ay = Mathf.Abs(vec.y);
        float az = Mathf.Abs(vec.z);
        float maxComponent = Mathf.Max(ax, Mathf.Max(ay, az));

        if (maxComponent == ax)
        {
            return (vec.x > 0) ? Vector3.right : Vector3.left;
        }
        else if (maxComponent == ay)
        {
            return (vec.y > 0) ? Vector3.up : Vector3.down;
        }
        else
        {
            return (vec.z > 0) ? Vector3.forward : Vector3.back;
        }
    }

    // Returns the Up vector of the current Gravity orientation
    public Vector3 GetGravityUpDirection()
    {
        return transform.up;
    }

    // Returns the cardinal forward vector for the Player
    private Vector3 GetViewForward()
    {
        return GetClosestCardinalVector(playerRef.forward);
    }

    // Returns the cardinal right vector for the Player
    private Vector3 GetViewRight(Vector3 cardinalForward)
    {
        Vector3 right = Vector3.Cross(playerRef.up, cardinalForward);
        return GetClosestCardinalVector(right);
    }

    void Update()
    {
        if (GameManager.canInput)
        {
            Vector3 forward = GetViewForward();
            Vector3 right = GetViewRight(forward);
            Vector3 up = transform.up;
            Quaternion lookRotation = transform.rotation;
            bool showHologram = false;

            // Find the new global up vector based on rotation
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                showHologram = true;
                lookRotation = Quaternion.LookRotation(forward, -right);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                showHologram = true;
                lookRotation = Quaternion.LookRotation(forward, right);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                showHologram = true;
                lookRotation = Quaternion.LookRotation(-up, forward);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                showHologram = true;
                lookRotation = Quaternion.LookRotation(up, -forward);
            }

            hologramRef.SetActive(showHologram);
            if (showHologram)
            {
                hologramRef.transform.rotation = lookRotation;
            }

            // Set the new up vector and rotate player
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                && showHologram)
            {
                transform.rotation = lookRotation;
                playerMovement.AlignWithGravity(lookRotation);
            }
        }
        else
        {
            hologramRef.SetActive(false);
        }
    }
}
