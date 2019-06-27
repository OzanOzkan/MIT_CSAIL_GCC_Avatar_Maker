using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for random avatar button.
/// </summary>
public class UIRandomAvatarButton : MonoBehaviour {

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
    public void OnButtonClick()
    {
        // Generate random avatar.
        AvatarCreatorContext.faceObject.GenerateRandomAvatar();

        // Update asset category list.
        AvatarCreatorContext.UpdateAssetCategoryList();

        // Log user action.
        AvatarCreatorContext.logManager.LogAction("UIButtonClick","CreateRandomAvatar");
    }
}
