using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollViewButtonController : MonoBehaviour
{
    public CBaseAsset faceAsset { get; set; }

    private void Start()
    {
        //gameObject.GetComponent<Image>().sprite = faceAsset.GetSprites()[SpritePart.Default];
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
        GameObject btnFaceObject = Instantiate(AvatarCreatorContext.faceObject.gameObject, gameObject.transform.Find("Panel"));
        btnFaceObject.transform.localScale = new Vector3(3, 3, 3);
        //btnFaceObject.transform.position = new Vector3(btnFaceObject.transform.position.x, -5f, btnFaceObject.transform.position.z);
        btnFaceObject.GetComponent<FaceObjectController>().SetFaceObjectPart(faceAsset, false);
    }

    public void OnButtonClick()
    {
        AvatarCreatorContext.faceObject.SetFaceObjectPart(faceAsset);

       //AvatarCreatorContext.logManager.LogAction("AssetSelected", faceAsset.GetSprites()[SpritePart.Default][0].name);
    }
}
