using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for realism level slider.
/// </summary>
public class UIRealismLevelController : MonoBehaviour {

    /// <summary>
    /// Called only once when application started.
    /// </summary>
    void Start () {
        // Register to value change event.
        gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    /// <summary>
    /// Called once in every frame.
    /// </summary>
    void Update () {

    }

    /// <summary>
    /// Called in every value change.
    /// </summary>
    public void ValueChangeCheck()
    {
        // Get the value and convert it to integer.
        int realismLevel = System.Convert.ToInt32(gameObject.GetComponent<Slider>().value);

        // Set current realism level.
        AvatarCreatorContext.currentRealismLevel = (RealismLevel)realismLevel;

        // Invoke SetFaceObjectPart. It will update the asset with current selected realism level.
        AvatarCreatorContext.faceObject.SetFaceObjectPart(null);

        // Update category list.
        AvatarCreatorContext.UpdateAssetCategoryList();

        // Log user action.
        AvatarCreatorContext.logManager.LogAction("RealismLevelChanged", AvatarCreatorContext.currentRealismLevel.ToString());
    }
}
