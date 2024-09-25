using UnityEngine;
using UnityEngine.UI;

public class CableController : MonoBehaviour
{
    public Transform hook;              // Reference to the hook
    public Transform trolley;           // Reference to the trolley
    public Transform concrete;          // Reference to the concrete attachment point
    public Transform cableModel;        // Reference to the cable model (assuming it stretches vertically)

    public Slider cableSlider;          // Reference to the UI Slider for cable length
    public float minCableLength = 0.1f;  // Minimum length of the cable
    public float maxCableLength = 2f;    // Maximum length of the cable
    public float movementScaleFactor = 5f;  // Scale factor to amplify hook movement

    private float currentCableLength;   // Current length of the cable

    void Start()
    {
        if (cableSlider != null)
        {
            // Set slider limits based on cable length
            cableSlider.minValue = 0f;  // Slider moves from 0 to 1 (normalized)
            cableSlider.maxValue = 1f;
            cableSlider.onValueChanged.AddListener(OnCableSliderChanged);

            // Initialize cable length based on current positions
            float initialCableLength = Vector3.Distance(hook.position, trolley.position);
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
        if (hook != null && trolley != null)
        {
            // Calculate the cable length based on the slider value (mapping it from min to max)
            currentCableLength = Mathf.Lerp(minCableLength, maxCableLength, cableSlider.value);

            // Move the hook vertically based on the cable length
            Vector3 hookPosition = trolley.position - new Vector3(0, currentCableLength, 0);

            // Apply the new position to the hook
            hook.position = hookPosition;

            // Update the cable's visual length
            UpdateCableVisual();
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


