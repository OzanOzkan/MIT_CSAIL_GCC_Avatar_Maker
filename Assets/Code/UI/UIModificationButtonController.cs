using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for asset modification buttons.
/// </summary>
public class UIModificationButtonController : MonoBehaviour {

    private AssetType m_lastSelectedAsset = AssetType.None;

    // Button type configured from Unity editor.
    public AssetModifyFlag m_buttonType;

    public bool positiveRate = true;

    /// <summary>
    /// Called only once when application started.
    /// </summary>
	void Start () {
        // Disabled by default.
        gameObject.GetComponent<Button>().interactable = false;

        // Register to button click events.
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    }

    /// <summary>
    /// Called once in every frame.
    /// </summary>
    void Update () {
        
        // Enable/disable the button according to current selected asset's modify flags.
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

    /// <summary>
    /// Called in every button press.
    /// </summary>
    public void OnButtonClick()
    {
        // Delegate the operation according to button type.
        if (m_buttonType == AssetModifyFlag.MoveHorizontal || m_buttonType == AssetModifyFlag.MoveVertical)
            AvatarCreatorContext.faceObject.MoveAsset(m_buttonType, positiveRate);
        else if (m_buttonType == AssetModifyFlag.Resize
                || m_buttonType == AssetModifyFlag.StretchVertical || m_buttonType == AssetModifyFlag.StretchHorizontal)
            AvatarCreatorContext.faceObject.ResizeAsset(m_buttonType, positiveRate);
        else if (m_buttonType == AssetModifyFlag.ChangeDistance)
            AvatarCreatorContext.faceObject.SetDistance(positiveRate);
        else if (m_buttonType == AssetModifyFlag.Rotate)
            AvatarCreatorContext.faceObject.RotateAsset(positiveRate);

        // Log user action.
        AvatarCreatorContext.logManager.LogAction("FaceAssetModified", m_buttonType.ToString() + positiveRate.ToString());
    }
}
