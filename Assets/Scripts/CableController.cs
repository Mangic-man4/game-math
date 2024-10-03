using UnityEngine;
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


