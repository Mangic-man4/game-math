using UnityEngine;
using UnityEngine.UI;

public class TrolleyController : MonoBehaviour
{
    public Transform craneTransform;        // Reference to the crane's rotating transform
    public Vector3 nearPointOffset;         // Offset for the near point relative to the crane
    public Vector3 farPointOffset;          // Offset for the far point relative to the crane

    [Range(0, 1)] public float trolleyPosition = 0.5f; // Normalized position of the trolley (0-1)

    public Slider trolleySlider; // Reference to the UI Slider

    void Start()
    {
        if (trolleySlider != null)
        {
            trolleySlider.value = trolleyPosition;
            trolleySlider.onValueChanged.AddListener(OnTrolleySliderChanged);
        }
    }

    void Update()
    {
        UpdateTrolleyPosition();
    }

    private void UpdateTrolleyPosition()
    {
        // Calculate the near and far points based on the crane's rotation
        Vector3 nearPoint = craneTransform.TransformPoint(nearPointOffset);
        Vector3 farPoint = craneTransform.TransformPoint(farPointOffset);

        // Interpolate the trolley's position between these points
        Vector3 localTrolleyPosition = Vector3.Lerp(nearPoint, farPoint, trolleyPosition);

        // Update the trolley's position in world space
        transform.position = localTrolleyPosition;
        transform.rotation = craneTransform.rotation; // Align rotation if needed
    }

    public void OnTrolleySliderChanged(float value)
    {
        trolleyPosition = value;
        UpdateTrolleyPosition();
    }
}






