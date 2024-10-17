using UnityEngine;
using UnityEngine.UI;

public class TrolleyController : MonoBehaviour
{
    public Transform concrete;
    public float moveSpeed = 2f;

    private bool moveToConcrete = false;
    private Vector3 targetPosition;

    //Old code
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
        if (moveToConcrete && !FindObjectOfType<CraneController>().isRotatingToConcrete) 
            {
                MoveTrolleyToConcrete();
            }
            else
            {
                UpdateTrolleyPosition();
            }
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
        transform.rotation = craneTransform.rotation; 
    }

    public void OnTrolleySliderChanged(float value)
    {
        trolleyPosition = value;
        UpdateTrolleyPosition();
    }
    public void StartMoveTrolleyToConcrete(Vector3 concretePosition)
    {

        // Calculate the horizontal position of the concrete (XZ plane)
        Vector3 projectedConcretePosition = new Vector3(concretePosition.x, transform.position.y, concretePosition.z);

        // Clamp the position between the near and far limits
        Vector3 nearPoint = craneTransform.TransformPoint(nearPointOffset);
        Vector3 farPoint = craneTransform.TransformPoint(farPointOffset);

        targetPosition = projectedConcretePosition;
        moveToConcrete = true;
    }

    public void MoveTrolleyToConcrete()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update the slider value based on the trolley's current position
        Vector3 nearPoint = craneTransform.TransformPoint(nearPointOffset);
        Vector3 farPoint = craneTransform.TransformPoint(farPointOffset);

        // Calculate the normalized position of the trolley between near and far points
        trolleyPosition = Mathf.InverseLerp(nearPoint.x, farPoint.x, transform.position.x);

        // Update the UI slider's value to reflect the trolley's movement
        if (trolleySlider != null)
        {
            trolleySlider.value = trolleyPosition;
        }

        // Check if the trolley has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            moveToConcrete = false;
            // Proceed to adjust the cable
            FindObjectOfType<CableController>().AdjustCableToConcrete();
        }
    }
}






