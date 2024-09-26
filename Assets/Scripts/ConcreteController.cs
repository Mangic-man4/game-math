using UnityEngine;

public class ConcreteController : MonoBehaviour
{
    public Transform hook; // Assign the hook transform in the inspector
    public Transform concreteAttachmentPoint; // Set the concrete's attachment point

    private bool isHookAttached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isHookAttached && other.CompareTag("ConcreteAttachment")) // Assuming the attachment point is tagged
        {
            AttachHookToConcrete();
        }
    }

    private void AttachHookToConcrete()
    {
        isHookAttached = true; // Mark the hook as attached
        hook.position = concreteAttachmentPoint.position; // Snap the hook to the concrete attachment point

        // Optionally stop cable length adjustment if the hook is attached
        // Example: GetComponent<CableController>().enabled = false; // Disable cable control if needed
    }

    public void ReleaseHook()
    {
        isHookAttached = false;

        // Logic to release the hook, allowing it to move again.
        // If you want to reset the position or other properties, do it here.
        // Example: hook.position = initialHookPosition; // Reset hook position if needed
    }
}

