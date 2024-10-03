using System.Collections;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    public Transform concrete;              // Reference to the concrete
    public Transform trolley;                // Reference to the trolley
    public Transform hook;                   // Reference to the hook
    public float rotationSpeed = 90f;       // Rotation speed in degrees per second
    public float moveSpeed = 2f;             // Trolley movement speed
    public float minHeight = 10f;           // Minimum height for the random position
    public float maxHeight = 20f;           // Maximum height for the random position
    public float trolleyNearLimit = 5f;     // Near limit for the trolley
    public float trolleyFarLimit = 15f;     // Far limit for the trolley
    public float waitTime = 1f;              // Wait time before lifting

    private bool isPickingUp = false;

    private void Update()
    {
        // Call this method when the user clicks on the concrete
        // For example, you can call it in response to a button click
    }

    public void OnConcreteClick()
    {
        if (!isPickingUp)
        {
            StartCoroutine(PickUpConcrete());
        }
    }

    private IEnumerator PickUpConcrete()
    {
        isPickingUp = true;

        // Step 1: Rotate Crane to Face Concrete
        RotateCraneToFaceConcrete();

        // Step 2: Move Trolley to Position
        MoveTrolleyToConcrete();

        // Step 3: Adjust Cable Length
        AdjustCableLength();

        // Step 4: Wait and Gradually Lift Concrete
        yield return new WaitForSeconds(waitTime);
        yield return StartCoroutine(LiftConcrete());

        // Step 5: Detach Concrete and Move to Random Location
        DetachConcrete();
        MoveConcreteToRandomLocation();

        isPickingUp = false;
    }

    private void RotateCraneToFaceConcrete()
    {
        Vector3 directionToConcrete = (concrete.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToConcrete);

        // Smoothly rotate to face the concrete
        StartCoroutine(RotateTo(targetRotation));
    }

    private IEnumerator RotateTo(Quaternion targetRotation)
    {
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
        while (angleDifference > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
            yield return null;
        }
    }

    private void MoveTrolleyToConcrete()
    {
        // Project the concrete position onto the XZ plane for trolley movement
        Vector3 projectedConcretePosition = new Vector3(concrete.position.x, trolley.position.y, concrete.position.z);
        float distanceToConcrete = Vector3.Distance(projectedConcretePosition, trolley.position);

        // Move trolley towards the concrete position while respecting limits
        if (distanceToConcrete > trolleyFarLimit)
        {
            // Trolley at far limit
            trolley.position += (projectedConcretePosition - trolley.position).normalized * trolleyFarLimit;
        }
        else if (distanceToConcrete < trolleyNearLimit)
        {
            // Trolley at near limit
            trolley.position += (projectedConcretePosition - trolley.position).normalized * trolleyNearLimit;
        }
        else
        {
            // Move trolley towards concrete
            trolley.position = Vector3.MoveTowards(trolley.position, projectedConcretePosition, moveSpeed * Time.deltaTime);
        }
    }

    private void AdjustCableLength()
    {
        // Calculate the distance to the concrete and adjust the cable length
        float distanceToConcrete = Vector3.Distance(hook.position, concrete.position);
        // Adjust cable length here, assuming you have a cable length setter
        // cableController.SetCableLength(distanceToConcrete);
    }

    private IEnumerator LiftConcrete()
    {
        float startY = concrete.position.y;
        float targetY = startY + (minHeight - startY); // Move up to minimum height

        while (concrete.position.y < targetY)
        {
            concrete.position += Vector3.up * (moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void DetachConcrete()
    {
        // Logic to detach the concrete from the hook
        // Example: concrete.SetParent(null); if it was parented
    }

    private void MoveConcreteToRandomLocation()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-10f, 10f), // X position
            Random.Range(minHeight, maxHeight), // Y position
            Random.Range(-10f, 10f)  // Z position
        );

        concrete.position = randomPosition;
    }
}





/*using UnityEngine;

public class CraneController : MonoBehaviour
{
    public float rotationSpeed = 30f; // Speed of rotation

    // Flags to determine if rotation should occur
    private bool rotateClockwise = false;
    private bool rotateCounterClockwise = false;

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


}*/
