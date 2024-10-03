using UnityEngine;

public class HookCollisionHandler : MonoBehaviour
{
    public CableController cableController;  // Reference to CableController

    void Start()
    {

        cableController = FindObjectOfType<CableController>();


        if (cableController == null)
        {
            Debug.LogWarning("CableController reference is not assigned in HookCollisionHandler. Assign it in the Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Concrete"))
        {
           // Debug.Log("Hook collided with Concrete. Notifying CableController.");
            cableController.OnHookCollisionWithConcrete();
        }
    }
}

