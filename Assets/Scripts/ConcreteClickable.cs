/*using UnityEngine;

public class ConcreteClickable : MonoBehaviour
{
    public CraneController craneController; // Reference to the CraneController

    void Start()
    {
        // Automatically find the CraneController in the scene
        if (craneController == null)
        {
            craneController = FindObjectOfType<CraneController>();
        }
    }

    void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Draw a ray in the scene for debugging
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 2f);

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object hit is the ConcreteClickableArea object
                if (hit.collider.CompareTag("ClickableArea")) // Use a tag for comparison
                {
                    Debug.Log("Concrete clicked!"); // Check if click detected
                    craneController.OnConcreteClicked(); // Call the crane action
                }
                else
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name); // Show which object was hit
                }
            }
        }
    }
}*/


