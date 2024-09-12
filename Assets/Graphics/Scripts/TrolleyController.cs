using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class TrolleyController : MonoBehaviour
{
    public Transform craneTransform;        // Reference to the crane's rotating transform
    public Transform trolleyNearLimit;      // Near point on the crane arm
    public Transform trolleyFarLimit;       // Far point on the crane arm

    [Range(0, 1)] public float trolleyPosition = 0.5f; // Normalized position of the trolley (0-1)

    public Slider trolleySlider; // Reference to the UI Slider

    void Start()
    {
        // If the slider is assigned, set its value and add a listener
        if (trolleySlider != null)
        {
            trolleySlider.value = trolleyPosition;
            trolleySlider.onValueChanged.AddListener(OnTrolleySliderChanged);
        }
    }

    void Update()
    {
        // Update trolley position relative to crane's rotation
        UpdateTrolleyPosition();
    }

    private void UpdateTrolleyPosition()
    {
        // Convert near and far limit positions to the crane's local space
        Vector3 localNearPoint = craneTransform.InverseTransformPoint(trolleyNearLimit.position);
        Vector3 localFarPoint = craneTransform.InverseTransformPoint(trolleyFarLimit.position);

        // Interpolate the trolley's position between these local points based on the normalized position
        Vector3 localTrolleyPosition = Vector3.Lerp(localNearPoint, localFarPoint, trolleyPosition);

        // Transform the interpolated local position back to world space using the crane's current transform
        Vector3 worldTrolleyPosition = craneTransform.TransformPoint(localTrolleyPosition);

        // Update the trolley's position
        transform.position = worldTrolleyPosition;

        // Optional: Align the trolley's rotation with the crane
        transform.rotation = craneTransform.rotation;

        // Debugging to verify positions
        Debug.Log($"Local Near Point: {localNearPoint}, Local Far Point: {localFarPoint}, World Trolley Position: {transform.position}");
    }

    // This method will be called when the slider value changes
    public void OnTrolleySliderChanged(float value)
    {
        // Update the normalized trolley position based on the slider value
        trolleyPosition = value;

        // Update the trolley’s position to reflect the new slider value
        UpdateTrolleyPosition();
    }
}




