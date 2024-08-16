using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages menu interactions such as reloading the current scene, switching to another scene, and exiting the game.
/// </summary>
public class MenuScript : MonoBehaviour
{
    /// <summary>
    /// Reloads the active scene.
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Switches the scene to the SelectionRoom scene.
    /// </summary>
    public void GoToSelectionRoom()
    {
        SceneManager.LoadScene("SelectionRoom");
    }

    /// <summary>
    /// Exits the game application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
