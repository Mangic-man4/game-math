using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CableController : MonoBehaviour
{
    public Transform hook;              // Reference to the hook
    public Transform trolley;           // Reference to the trolley
    public Transform concrete;          // Reference to the concrete attachment point
    public Transform cableModel;        // Reference to the cable model
    public Slider cableSlider;          // Reference to the UI Slider for cable length
    public float minCableLength = 0.1f;  // Minimum length of the cable
    public float maxCableLength = 2f;   // Maximum length of the cable
    public float movementScaleFactor = 1f;  // Scale factor to amplify hook movement
    public float rotationSpeed = 90f;    // Crane rotation speed
    public float moveSpeed = 2f;         // Trolley movement speed
    public float minHeight = 10f;        // Minimum height for the random position
    public float maxHeight = 20f;        // Maximum height for the random position
    public float trolleyNearLimit = 5f;  // Near limit for the trolley
    public float trolleyFarLimit = 15f;  // Far limit for the trolley
    public float waitTime = 1f;          // Wait time before lifting

    private float currentCableLength;    // Current length of the cable
    private bool isPickingUp = false;    // Flag to check if picking up concrete

    void Start()
    {
        if (cableSlider != null)
        {
            // Set slider limits based on cable length
            cableSlider.minValue = 0f;  // Slider moves from 0 to 1 
            cableSlider.maxValue = 1f;
            cableSlider.onValueChanged.AddListener(OnCableSliderChanged);
            InitializeCableLength();
        }
    }

    void Update()
    {
        if (cableSlider != null)
        {
            UpdateCableAndHook();
        }
    }

    private void InitializeCableLength()
    {
        // Initialize cable length based on current positions
        float initialCableLength = Mathf.Clamp(Vector3.Distance(hook.position, trolley.position), minCableLength, maxCableLength);
        float normalizedLength = Mathf.InverseLerp(minCableLength, maxCableLength, initialCableLength);
        cableSlider.value = normalizedLength;
    }

    private void UpdateCableAndHook()
    {
        if (hook != null && trolley != null)
        {
            // Calculate the cable length based on the slider value
            currentCableLength = Mathf.Lerp(minCableLength, maxCableLength, cableSlider.value);
            float hookYPosition = Mathf.Lerp(40f, 12.5f, (currentCableLength - minCableLength) / (maxCableLength - minCableLength));

            // Update the hook's position
            hook.position = new Vector3(trolley.position.x, hookYPosition, trolley.position.z);
            // Update the cable's position
            cableModel.position = new Vector3(trolley.position.x, trolley.position.y, trolley.position.z);
            // Update the cable's scale
            cableModel.localScale = new Vector3(cableModel.localScale.x, currentCableLength, cableModel.localScale.z);
        }
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
            trolley.position += (projectedConcretePosition - trolley.position).normalized * trolleyFarLimit;
        }
        else if (distanceToConcrete < trolleyNearLimit)
        {
            trolley.position += (projectedConcretePosition - trolley.position).normalized * trolleyNearLimit;
        }
        else
        {
            trolley.position = Vector3.MoveTowards(trolley.position, projectedConcretePosition, moveSpeed * Time.deltaTime);
        }
    }

    private void AdjustCableLength()
    {
        // Calculate the distance to the concrete and adjust the cable length
        float distanceToConcrete = Vector3.Distance(hook.position, concrete.position);
        // Assuming you have a method to set the cable length, you can set it here
        currentCableLength = Mathf.Clamp(distanceToConcrete, minCableLength, maxCableLength);
        cableSlider.value = Mathf.InverseLerp(minCableLength, maxCableLength, currentCableLength);
    }

    private IEnumerator LiftConcrete()
    {
        float startY = concrete.position.y;
        float targetY = startY + (minCableLength - startY); // Move up to minimum height

        while (concrete.position.y < targetY)
        {
            concrete.position += Vector3.up * (moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void DetachConcrete()
    {
        // Logic to detach the concrete from the hook
        // Here you can perform any detachment logic you need
        // e.g., concrete.SetParent(null); if it was parented
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

    public void OnCableSliderChanged(float value)
    {
        // Update the cable length based on the slider value
        currentCableLength = Mathf.Lerp(minCableLength, maxCableLength, value);
        UpdateCableAndHook();
    }
}





/*using UnityEngine;
using UnityEngine.UI;

public class CableController : MonoBehaviour
{
    public Transform hook;              // Reference to the hook
    public Transform trolley;           // Reference to the trolley
    public Transform concrete;          // Reference to the concrete attachment point
    public Transform cableModel;        // Reference to the cable model
    public Transform cable;

    public Slider cableSlider;          // Reference to the UI Slider for cable length
    public float minCableLength = 0.1f; // Minimum length of the cable
    public float maxCableLength = 2f;   // Maximum length of the cable
    public float hookMinY = 12.5f;      // Minimum Y position of the hook
    public float hookMaxY = 40f;        // Maximum Y position of the hook

    private float currentCableLength;   // Current length of the cable
    private bool isHookAttachedToConcrete = false; // Track if the hook is attached

    void Start()
    {
        if (cableSlider != null)
        {
            cableSlider.minValue = 0f;
            cableSlider.maxValue = 1f;
            cableSlider.onValueChanged.AddListener(OnCableSliderChanged);

            // Initialize cable length based on current positions
            float initialCableLength = Mathf.Clamp(Vector3.Distance(hook.position, trolley.position), minCableLength, maxCableLength);
            float normalizedLength = Mathf.InverseLerp(minCableLength, maxCableLength, initialCableLength);
            cableSlider.value = normalizedLength;
        }
    }

    void Update()
    {
        // Always update the cable and hook
        UpdateCableAndHook();
    }

    private void UpdateCableAndHook()
    {
        if (hook != null && trolley != null && cable != null)
        {
            // Calculate the cable length based on the slider value
            currentCableLength = Mathf.Lerp(minCableLength, maxCableLength, cableSlider.value);

            // Calculate the vertical position for the hook based on the cable length
            float hookYPosition = Mathf.Lerp(hookMaxY, hookMinY, (currentCableLength - minCableLength) / (maxCableLength - minCableLength));

            // Update the hook's position based on the trolley's position
            if (!isHookAttachedToConcrete)
            {
                // Only update the hook's position if not attached to the concrete
                hook.position = new Vector3(trolley.position.x, hookYPosition, trolley.position.z);
            }
            else
            {
                // If attached, keep the hook's position but allow the concrete to follow
                hook.position = new Vector3(trolley.position.x, hookYPosition, trolley.position.z);
                // Move the concrete to follow the hook, but keep the concrete's X and Z position consistent with the trolley
                concrete.position = new Vector3(trolley.position.x, hook.position.y, trolley.position.z);
            }

            // Update the cable's position
            cable.position = new Vector3(trolley.position.x, trolley.position.y, trolley.position.z);
            cable.localScale = new Vector3(cable.localScale.x, currentCableLength, cable.localScale.z);
        }
    }

    // This method is called by HookCollisionHandler when the hook collides with the concrete
    public void OnHookCollisionWithConcrete()
    {
        if (!isHookAttachedToConcrete)
        {
            AttachHookToConcrete();
        }
    }

    private void AttachHookToConcrete()
    {
        // Snap the hook to the concrete attachment point
        hook.position = concrete.position;
        isHookAttachedToConcrete = true;

        // Keep the concrete's position in sync with the hook's current position
        concrete.position = hook.position;
    }

    public void OnCableSliderChanged(float value)
    {
        // Update the cable length based on the slider value
        currentCableLength = Mathf.Lerp(minCableLength, maxCableLength, value);
        UpdateCableAndHook();
    }
}
*/

