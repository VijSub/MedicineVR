using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

/// <summary>
/// Manages the creation and configuration of a bounding box around a cube.
/// </summary>
public class BounderBox : MonoBehaviour
{
    /// <summary>
    /// Reference to the instantiated cube.
    /// </summary>
    private GameObject cube;

    /// <summary>
    /// Reference to the BoundingBox component attached to the cube.
    /// </summary>
    private BoundingBox bbox;

    /// <summary>
    /// Start is called before the first frame update.
    /// Initializes the cube and its bounding box with specific settings.
    /// </summary>
    void Start()
    {
        // Instantiate a cube GameObject
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Optionally set the position, scale, and other properties of the cube
        cube.transform.position = new Vector3(0, 1, 0);  // Example position
        cube.transform.localScale = new Vector3(1, 1, 1);  // Example scale
        cube.name = "BoundingCube";  // Set a name for the cube

        // Set the cube as a child of the current GameObject (optional)
        cube.transform.parent = transform;

        // Ensure the cube has a collider
        if (cube.GetComponent<Collider>() == null)
        {
            cube.AddComponent<BoxCollider>();
        }

        // Assign BoundingBox script to the cube
        bbox = cube.AddComponent<BoundingBox>();

        // Configure BoundingBox properties
        bbox.BoundingBoxActivation = BoundingBox.BoundingBoxActivationType.ActivateOnStart;

        // Make the scale handles large
        bbox.ScaleHandleSize = 0.1f;

        // Hide rotation handles
        bbox.ShowRotationHandleForX = false;
        bbox.ShowRotationHandleForY = false;
        bbox.ShowRotationHandleForZ = false;

        // Ensure MinMaxScaleConstraint is added and configure it
        MinMaxScaleConstraint scaleConstraint = bbox.gameObject.GetComponent<MinMaxScaleConstraint>();
        if (scaleConstraint == null)
        {
            scaleConstraint = bbox.gameObject.AddComponent<MinMaxScaleConstraint>();
        }
        scaleConstraint.ScaleMinimum = 1f;
        scaleConstraint.ScaleMaximum = 2f;
    }

    /// <summary>
    /// Update is called once per frame.
    /// Add any update logic if necessary.
    /// </summary>
    void Update()
    {
        // Add any update logic if necessary
    }
}
