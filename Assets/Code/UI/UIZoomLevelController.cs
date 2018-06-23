using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZoomLevelController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ValueChangeCheck()
    {
        float sliderValue = gameObject.GetComponent<Slider>().value;

        AvatarCreatorContext.faceObject.transform.localScale = new Vector3(sliderValue, sliderValue, 0);
        //AvatarCreatorContext.faceObject.transform.position = new Vector3(AvatarCreatorContext.faceObject.transform.position.x, AvatarCreatorContext.faceObject.transform.position.y - (sliderValue*2), 0);
    }
}
