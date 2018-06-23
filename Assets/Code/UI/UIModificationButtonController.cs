using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModificationButtonController : MonoBehaviour {

    private AssetType m_lastSelectedAsset = AssetType.None;
    public AssetModifyFlag m_buttonType;
    public bool positiveRate = true;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    }
	
	// Update is called once per frame
	void Update () {
        
        if(m_lastSelectedAsset != AvatarCreatorContext.selectedAssetType)
        {
            CBaseAsset checkAsset = AvatarCreatorContext.GetLoadedAssetsByType(AvatarCreatorContext.selectedAssetType)[0];

            if ((checkAsset.GetModifyFlags() & m_buttonType) != 0)
            {
                gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                gameObject.GetComponent<Button>().interactable = false;
            }

            m_lastSelectedAsset = AvatarCreatorContext.selectedAssetType;
        }
	}

    public void OnButtonClick()
    {
        if (m_buttonType == AssetModifyFlag.MoveHorizontal || m_buttonType == AssetModifyFlag.MoveVertical)
            AvatarCreatorContext.faceObject.MoveAsset(m_buttonType, positiveRate);
        else if (m_buttonType == AssetModifyFlag.Resize
                || m_buttonType == AssetModifyFlag.StretchVertical || m_buttonType == AssetModifyFlag.StretchHorizontal)
            AvatarCreatorContext.faceObject.ResizeAsset(m_buttonType, positiveRate);
        else if (m_buttonType == AssetModifyFlag.ChangeDistance)
            AvatarCreatorContext.faceObject.SetDistance(positiveRate);
        else if (m_buttonType == AssetModifyFlag.Rotate)
            AvatarCreatorContext.faceObject.RotateAsset(positiveRate);

        AvatarCreatorContext.logManager.LogAction("FaceAssetModified", m_buttonType.ToString() + positiveRate.ToString());
    }
}
