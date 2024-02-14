using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    public float pointSpacing = 0.1f; // Spacing between points on the line

    private void Update()
    {
        // Player rotation
        float rotationInput = Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationInput, 0);

        // Player movement
        float movementInput = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, movementInput);

        // Update line renderer
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        // Add new point to the line renderer
        if (lineRenderer.positionCount == 0 || Vector3.Distance(transform.position, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > pointSpacing)
        {
            lineRenderer.positionCount++;
            Vector3 pos = transform.position;
            pos.y = 0;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
        }
    }
}
