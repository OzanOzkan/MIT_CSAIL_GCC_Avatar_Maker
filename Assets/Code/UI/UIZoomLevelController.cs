using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for zoom slider.
/// </summary>
public class UIZoomLevelController : MonoBehaviour {

    float oldSliderValue;

    /// <summary>
    /// Called only once when application started.
    /// </summary>
	void Start () {
		gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        oldSliderValue = gameObject.GetComponent<Slider>().value;
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
        // Get current slider value.
        float sliderValue = gameObject.GetComponent<Slider>().value;

        // Adjust face object scale.
        AvatarCreatorContext.faceObject.transform.localScale = new Vector3(sliderValue, sliderValue, 0);

        // Calculate new position of the face object according to the new zoom level (scale).
        float level = ((280 - 0) / (gameObject.GetComponent<Slider>().maxValue - gameObject.GetComponent<Slider>().minValue)) * (gameObject.GetComponent<Slider>().maxValue - gameObject.GetComponent<Slider>().value);

        // Set position.
        AvatarCreatorContext.faceObject.transform.position = new Vector3(AvatarCreatorContext.faceObject.transform.position.x, level, 0);
    }
}
