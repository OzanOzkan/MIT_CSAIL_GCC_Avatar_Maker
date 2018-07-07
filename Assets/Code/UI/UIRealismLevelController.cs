using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRealismLevelController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void ValueChangeCheck()
    {
        int realismLevel = System.Convert.ToInt32(gameObject.GetComponent<Slider>().value);
        AvatarCreatorContext.currentRealismLevel = (RealismLevel)realismLevel;
        AvatarCreatorContext.faceObject.SetFaceObjectPart(null);

        AvatarCreatorContext.logManager.LogAction("RealismLevelChanged", AvatarCreatorContext.currentRealismLevel.ToString());
    }
}
