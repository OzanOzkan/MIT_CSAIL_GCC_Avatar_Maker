using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollViewButtonController : MonoBehaviour
{
    public CBaseAsset faceAsset { get; set; }

    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = faceAsset.GetSprites()[SpritePart.Default];
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    }

    public void OnButtonClick()
    {
        AvatarCreatorContext.faceObject.SetFaceObjectPart(faceAsset);

        AvatarCreatorContext.logManager.LogAction("AssetSelected", faceAsset.GetSprites()[SpritePart.Default].name);
    }
}
