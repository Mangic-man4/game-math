using UnityEngine;

public class OffsetCalculator : MonoBehaviour
{
    public Transform craneTransform;
    public Transform nearPointObject; // The GameObject representing the near point
    public Transform farPointObject;  // The GameObject representing the far point

    public Vector3 nearPointOffset;
    public Vector3 farPointOffset;

    void Start()
    {
        // Calculate offsets relative to the crane's transform
        nearPointOffset = craneTransform.InverseTransformPoint(nearPointObject.position);
        farPointOffset = craneTransform.InverseTransformPoint(farPointObject.position);

//        Debug.Log($"Near Point Offset: {nearPointOffset}");
//        Debug.Log($"Far Point Offset: {farPointOffset}");
    }
}

