using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class CableController : MonoBehaviour
{
    public Transform hook;              // Reference to the hook
    public Transform trolley;           // Reference to the trolley
    public Transform concrete;          // Reference to the concrete attachment point
    public Transform cableModel;        // Reference to the cable model

    public Slider cableSlider;          // Reference to the UI Slider for cable length
    public float minCableLength = 1f;  // Minimum length of the cable
    public float maxCableLength = 10f; // Maximum length of the cable

    private float currentCableLength;  // Current length of the cable

    void Start()
    {
        // Initialize the slider value
        if (cableSlider != null)
        {
            cableSlider.minValue = minCableLength;
            cableSlider.maxValue = maxCableLength;
            cableSlider.onValueChanged.AddListener(OnCableSliderChanged);

            // Set the initial value based on current cable length
            currentCableLength = Mathf.Clamp(hook.position.y - trolley.position.y, minCableLength, maxCableLength);
            cableSlider.value = currentCableLength;
        }
    }

    void Update()
    {
        // Update the cable length based on the slider value
        if (cableSlider != null)
        {
            currentCableLength = cableSlider.value;
            UpdateCableVisual();
        }

        // Check if the hook is colliding with the concrete
        if (Vector3.Distance(hook.position, concrete.position) < 0.1f) // Collision threshold
        {
            AttachHookToConcrete();
        }
    }

    private void UpdateCableVisual()
    {
        // Adjust the cable model length
        if (cableModel != null)
        {
            Vector3 scale = cableModel.localScale;
            scale.y = currentCableLength; // Assuming Y-axis is the cable length
            cableModel.localScale = scale;
        }
    }

    public void OnCableSliderChanged(float value)
    {
        // Update the cable length based on the slider value
        currentCableLength = Mathf.Clamp(value, minCableLength, maxCableLength);
        UpdateCableVisual();
    }

    private void AttachHookToConcrete()
    {
        // Snap the hook to the concrete attachment point
        hook.position = concrete.position;
        // Optionally, stop cable length adjustment if the hook is attached
    }
}


