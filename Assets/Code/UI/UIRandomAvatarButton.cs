using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRandomAvatarButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnButtonClick()
    {
        AvatarCreatorContext.faceObject.GenerateRandomAvatar();
        AvatarCreatorContext.UpdateAssetCategoryList();
        AvatarCreatorContext.logManager.LogAction("UIButtonClick","CreateRandomAvatar");
    }
}
