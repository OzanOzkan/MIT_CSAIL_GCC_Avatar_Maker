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
        btnFaceObject.transform.localPosition = new Vector3(0, 0, 0);
        btnFaceObject.transform.localScale = new Vector3(5, 5, 5);
        btnFaceObject.GetComponent<FaceObjectController>().SetFaceObjectPart(faceAsset, false);
        gameObject.transform.Find("Panel").GetComponent<Image>().color = AvatarCreatorContext.bgColor;
    }

    public void OnButtonClick()
    {
        AvatarCreatorContext.faceObject.SetFaceObjectPart(faceAsset);
        AvatarCreatorContext.selectedAssetType = faceAsset.GetAssetType();

       //AvatarCreatorContext.logManager.LogAction("AssetSelected", faceAsset.GetSprites()[SpritePart.Default][0].name);
    }
}
