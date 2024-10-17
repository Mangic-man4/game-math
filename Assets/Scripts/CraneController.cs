using UnityEngine;

public class CraneController : MonoBehaviour
{
    //Old code
    public float rotationSpeed = 30f; // Speed of rotation
    public Transform concrete; // Reference to the concrete

    public bool isRotatingToConcrete = false;
    private Quaternion targetRotation;
    private Vector3 concretePosition;

    // Flags to determine if rotation should occur
    private bool rotateClockwise = false;
    private bool rotateCounterClockwise = false;
    //private float logTimer = 0f;        // Timer for logging
    //public float logInterval = 1f;     // Log interval in seconds

    void Update()
    {
        // Rotate the crane continuously if the flags are set
        if (rotateClockwise)
        {
            RotateCraneClockwise();
        }
        else if (rotateCounterClockwise)
        {
            RotateCraneCounterClockwise();
        }
        // Rotate the crane continuously if the flags are set
        if (isRotatingToConcrete)
        {
            RotateCraneTowardsConcrete();
        }

    }

    // Trigger crane rotation towards concrete
    public void StartRotationTowardsConcrete(Vector3 targetConcretePosition)
    {

        // Ensure the concrete reference is valid
        if (concrete == null)
        {
            Debug.LogError("Concrete object reference is missing!");
            return;
        }

        // Calculate the direction towards the concrete's position in the XZ plane
        Vector3 directionToConcrete = (concrete.position - transform.position).normalized;

        // Zero out the Y component so the crane only rotates on the horizontal axis (XZ plane)
        directionToConcrete.y = 0f;

        // Set the target rotation to face the concrete
        if (directionToConcrete != Vector3.zero) // Prevent LookRotation from breaking if direction is zero
        {
            targetRotation = Quaternion.LookRotation(directionToConcrete);
            targetRotation *= Quaternion.Euler(0, 90, 0); //LOL, all i had to add was this to get it to work... Nice 6+ hours wasted
            isRotatingToConcrete = true;
        }
        else
        {
            Debug.LogWarning("Concrete is directly above or below the crane, cannot rotate.");
        }

    }

    private void RotateCraneTowardsConcrete()
    {
        // Smoothly rotate to face the concrete
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            // Stop rotating once close to the target
            isRotatingToConcrete = false;
            // Trigger the next step after rotation - Move the trolley
            FindObjectOfType<TrolleyController>().StartMoveTrolleyToConcrete(concrete.position);
        }
    }

    // Methods to be called by the UI buttons
    public void StartRotateClockwise()
    {
        rotateClockwise = true;
    }

    public void StopRotateClockwise()
    {
        rotateClockwise = false;
    }

    public void StartRotateCounterClockwise()
    {
        rotateCounterClockwise = true;
    }

    public void StopRotateCounterClockwise()
    {
        rotateCounterClockwise = false;
    }

    private void RotateCraneClockwise()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void RotateCraneCounterClockwise()
    {
        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
    }


}

