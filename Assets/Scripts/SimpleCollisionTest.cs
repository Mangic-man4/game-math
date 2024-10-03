using UnityEngine;

public class SimpleCollisionTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {       
        //Debug.Log($"{gameObject.name} entered trigger with {other.gameObject.name}");
    }

    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}");
    }
}
