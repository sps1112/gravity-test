using UnityEngine;

public class CubeCollectible : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name=="Player")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateScore(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
