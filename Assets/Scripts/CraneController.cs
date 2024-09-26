using UnityEngine;

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


}
