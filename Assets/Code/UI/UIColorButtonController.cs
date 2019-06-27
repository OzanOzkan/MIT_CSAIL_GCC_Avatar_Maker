using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for every color (button) located in the color palette.
/// </summary>
public class UIColorButtonController : MonoBehaviour {

    /// <summary>
    /// Called only once when application started.
    /// </summary>
    void Start () {
        
        // Register to button click events.
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    }

    /// <summary>
    /// Called once in every frame.
    /// </summary>
    void Update () {
		
	}

    /// <summary>
    /// Called in every button press.
    /// </summary>
    void OnButtonClick()
    {
        // Change asset color to selected color. (GetComponent<Image>() returns this gameObject's image and .color returns it's color)
        AvatarCreatorContext.faceObject.ChangeAssetColor(gameObject.GetComponent<Image>().color);
    }
}
