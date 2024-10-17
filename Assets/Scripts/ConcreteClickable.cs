using UnityEngine;

public class ConcreteClickable : MonoBehaviour
{
    public CraneController craneController; // Reference to the CraneController
    public TrolleyController trolleyController; // Reference to the TrolleyController

    void Start()
    {
        // Automatically find the CraneController in the scene
        if (craneController == null)
        {
            craneController = FindObjectOfType<CraneController>();
        }

        // Automatically find the TrolleyController in the scene
        if (trolleyController == null)
        {
            trolleyController = FindObjectOfType<TrolleyController>();
        }

    }

    void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object hit has the "Concrete" tag
                if (hit.collider.CompareTag("Concrete")) // Make sure the concrete object is tagged as "Concrete"
                {
                   // Debug.Log("Concrete clicked!"); // Check if click detected
                    Vector3 concretePosition = hit.collider.transform.position;
                    craneController.StartRotationTowardsConcrete(concretePosition); // Call the crane action with the concrete position
                    trolleyController.StartMoveTrolleyToConcrete(concretePosition); // Call the trolley action
                }
                else
                {
                    Debug.Log("Clicked on: " + hit.collider.gameObject.name); // For debugging, print what was clicked
                }
            }
        }
    }
}




