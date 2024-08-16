using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// defines static utility methods for unity
/// </summary>
public static class StaticUtils
{
    /// <summary>
    /// FindObject for identifying disabled objects
    /// </summary>
    /// <param name="parent">Parent of disabled object</param>
    /// <param name="name">Name of the child object to be enabled</param>
    /// <returns></returns>
    public static GameObject FindObject(this GameObject parent, string name){
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs){
            if(t.name == name){
                return t.gameObject;
            }
        }
        return null;
    }
}
