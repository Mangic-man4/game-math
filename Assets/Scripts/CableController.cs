using UnityEngine;
using UnityEngine.UI;

public class CableController : MonoBehaviour
{
    public Transform hook;              // Reference to the hook
    public Transform trolley;           // Reference to the trolley
    public Transform concrete;          // Reference to the concrete attachment point
    public Transform cableModel;        // Reference to the cable model (assuming it stretches vertically
    public Transform cable;

    public Slider cableSlider;          // Reference to the UI Slider for cable length
    public float minCableLength = 0.1f;  // Minimum length of the cable
    public float maxCableLength = 2f;   // Maximum length of the cable
    public float movementScaleFactor = 1f;  // Scale factor to amplify hook movement (if needed)

    private float currentCableLength;   // Current length of the cable
    private float logTimer = 0f;        // Timer for logging
    public float logInterval = 0.5f;     // Log interval in seconds

    void Start()
    {
        if (cableSlider != null)
        {
            // Set slider limits based on cable length
            cableSlider.minValue = 0f;  // Slider moves from 0 to 1 (normalized)
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
        if (cableSlider != null)
        {
            UpdateCableAndHook();
        }

        // Check if the hook is colliding with the concrete (attachment logic)
        if (Vector3.Distance(hook.position, concrete.position) < 0.1f)
        {
            AttachHookToConcrete();
        }
    }
    private void UpdateCableAndHook()
    {
        if (hook != null && trolley != null && cable != null)
        {
            // Calculate the cable length based on the slider value
            currentCableLength = Mathf.Lerp(minCableLength, maxCableLength, cableSlider.value);

            // Calculate the vertical position for the hook
            float hookYPosition = Mathf.Lerp(40f, 12.5f, (currentCableLength - minCableLength) / (maxCableLength - minCableLength));

            // Update the hook's position
            hook.position = new Vector3(trolley.position.x, hookYPosition, trolley.position.z);

            // Update the cable's position
            cable.position = new Vector3(trolley.position.x, trolley.position.y, trolley.position.z); // Match the trolley's position

            // Update the cable's scale
            cable.localScale = new Vector3(cable.localScale.x, currentCableLength, cable.localScale.z); // Maintain original X and Z scales

            /* Log debug information
            logTimer += Time.deltaTime; // Increment timer by the time since the last frame
            if (logTimer >= logInterval) // Check if the timer exceeds the log interval
            {
                Debug.Log($"Trolley Position: {trolley.position}, Current Cable Length: {currentCableLength}, Hook Position: {hook.position}, Cable Position: {cable.position}, Cable Scale: {cable.localScale}");
                logTimer = 0f; // Reset the log timer
            }*/
        }
    }



    private void UpdateCableVisual()
    {
        if (cableModel != null && hook != null && trolley != null)
        {
            // Adjust cable's position to match the trolley's position (cable should start at the trolley)
            cableModel.position = trolley.position;

            // Calculate the real distance between the trolley and the hook
            float cableLength = Vector3.Distance(trolley.position, hook.position);

            // Adjust the cable's scale to match the real distance (without movement scaling)
            Vector3 cableScale = cableModel.localScale;
            cableScale.y = cableLength;  // Set the cable's vertical stretch to reflect actual length
            cableModel.localScale = cableScale;

            // Calculate the direction from trolley to hook, but only adjust Y rotation
            Vector3 directionToHook = (hook.position - trolley.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToHook);

            // Apply only the Y-axis rotation to the cable (ignoring X and Z rotations)
            cableModel.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        }
    }

    public void OnCableSliderChanged(float value)
    {
        // Update the cable length based on the slider value
        currentCableLength = Mathf.Lerp(minCableLength, maxCableLength, value);
        UpdateCableAndHook();
    }

    private void AttachHookToConcrete()
    {
        // Snap the hook to the concrete attachment point
        hook.position = concrete.position;
        // Optionally, stop cable length adjustment if the hook is attached
    }
}

