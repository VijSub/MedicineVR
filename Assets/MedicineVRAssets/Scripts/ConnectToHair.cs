using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attaches the game object to a specified SkinnedMeshRenderer representing hair.
/// </summary>
public class ConnectToHair : MonoBehaviour
{
    /// <summary>
    /// The SkinnedMeshRenderer representing the hair to which this game object will attach.
    /// </summary>
    [SerializeField]
    private SkinnedMeshRenderer Hair;

    /// <summary>
    /// Updates the position of the game object to be just above the center of the hair.
    /// </summary>
    void Update()
    {
        // Offset of 0.085f so the object is positioned just above the hair's center
        this.transform.position = new Vector3(Hair.bounds.center.x, Hair.bounds.center.y + 0.085f, Hair.bounds.center.z);
    }
}
