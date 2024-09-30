using UnityEngine;

public class ConcreteController : MonoBehaviour
{
    public Transform hook; // Assign the hook transform in the inspector
    public Transform concreteAttachmentPoint; // Set the concrete's attachment point

    private bool isHookAttached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isHookAttached && other.CompareTag("ConcreteAttachment")) //Remember tag on gameobject
        {
            AttachHookToConcrete();
        }
    }

    private void AttachHookToConcrete()
    {
        isHookAttached = true; // Mark the hook as attached
        hook.position = concreteAttachmentPoint.position; // Snap the hook to the concrete attachment point
    }

    public void ReleaseHook()
    {
        isHookAttached = false;
    }
}

