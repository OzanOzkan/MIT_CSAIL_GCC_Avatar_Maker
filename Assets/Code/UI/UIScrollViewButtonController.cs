using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for scroll view buttons (assets in the list).
/// </summary>
public class UIScrollViewButtonController : MonoBehaviour
{
    public CBaseAsset faceAsset { get; set; }

    /// <summary>
    /// Called only once when application started.
    /// </summary>
    private void Start()
    {
        // Register to button click events.
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());

        // Button configuration (position, scale, background etc.)
        GameObject btnFaceObject = Instantiate(AvatarCreatorContext.faceObject.gameObject.transform.parent.gameObject, gameObject.transform.Find("Panel"));
        btnFaceObject.transform.localPosition = new Vector3(0, 0, 0);
        btnFaceObject.transform.localScale = new Vector3(5, 5, 5);
        btnFaceObject.transform.Find("FaceObject").GetComponent<FaceObjectController>().SetFaceObjectPart(faceAsset, false);
        btnFaceObject.transform.Find("FaceObject").localPosition = new Vector3(0, 0, 0);
        btnFaceObject.transform.Find("FaceObject").localScale = new Vector3(1, 1, 1);
        btnFaceObject.transform.Find("bg_texture").localPosition = new Vector3(0, 0, 0);
        btnFaceObject.transform.Find("bg_texture").GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        gameObject.transform.Find("Panel").GetComponent<Image>().color = AvatarCreatorContext.bgColor;
    }

    /// <summary>
    /// Called in every value change.
    /// </summary>
    public void OnButtonClick()
    {
        // Send selected asset to face object. It will change the face.
        AvatarCreatorContext.faceObject.SetFaceObjectPart(faceAsset);

        // Set global selected asset type variable to currently selected asset type.
        AvatarCreatorContext.selectedAssetType = faceAsset.GetAssetType();
    }
}
