using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZoomLevelController : MonoBehaviour {

    float oldSliderValue;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        oldSliderValue = gameObject.GetComponent<Slider>().value;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ValueChangeCheck()
    {
        float sliderValue = gameObject.GetComponent<Slider>().value;

        AvatarCreatorContext.faceObject.transform.localScale = new Vector3(sliderValue, sliderValue, 0);

        float test = ((280 - 0) / (gameObject.GetComponent<Slider>().maxValue - gameObject.GetComponent<Slider>().minValue)) * (gameObject.GetComponent<Slider>().maxValue - gameObject.GetComponent<Slider>().value);


        //float final = ((sliderValue - 20f) * (280 / 130));

        //if (sliderValue < oldSliderValue)
        //{
            AvatarCreatorContext.faceObject.transform.position = new Vector3(AvatarCreatorContext.faceObject.transform.position.x, test, 0);
      //  }

        //oldSliderValue = sliderValue;

        //  Mathf.Clamp()

        //AvatarCreatorContext.faceObject.transform.position = new Vector3(AvatarCreatorContext.faceObject.transform.position.x, AvatarCreatorContext.faceObject.transform.position.y - (sliderValue*2), 0);
    }
}
