using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.UI;

/// <summary>
/// Defines the Object to which the script is applied as button which will change the Scene
/// </summary>
public class VRButton : MonoBehaviour
{
    // register a listener on the object behaving like a button
    private void OnEnable()
    {
        // Registriere das Event
        var interactable = GetComponent<Interactable>();
        interactable.OnClick.AddListener(OnButtonClicked);
    }

    // deregister a listener on the object behaving like a button
    private void OnDisable()
    {
        // Deregistriere das Event
        var interactable = GetComponent<Interactable>();
        interactable.OnClick.RemoveListener(OnButtonClicked);
    }

    // Change Scene on Button clicked
    private void OnButtonClicked()
    {
        // Name des Buttons (GameObjects) ermitteln
        string buttonName = gameObject.name;

        // Szenenname basierend auf dem Button-Namen festlegen
        string sceneName = buttonName + "Scene";

        // Wechsel zur angegebenen Szene
        Debug.Log("Button ber�hrt! Wechsel zu Szene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
