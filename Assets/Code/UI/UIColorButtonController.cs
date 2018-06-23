using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnButtonClick()
    {
        AvatarCreatorContext.faceObject.ChangeAssetColor(gameObject.GetComponent<Image>().color);
    }
}
